using System.Security.Claims;
using System.Security.Cryptography;
using Client.Features.Auth;
using Client.Features.Files;
using ErrorOr;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Data;
using Server.Features.Auth;
using Server.Infrastructure;
using FileHashString = Client.Features.Files.FileHashString;
using FileIdentifier = Client.Features.Files.FileIdentifier;
using FileSize = Client.Features.Files.FileSize;

namespace Server.Features.Files;

[Handler]
[MapPost(IFileClient.UploadFilePath)]
public partial class UploadFile
{
    internal static Results<Ok<FileModel>, ProblemHttpResult> TransformResult(
        ErrorOr<FileModel> value) => value.GetHttpResult();
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint)
        => endpoint.DisableAntiforgery().RequireAuthorization().WithTags(nameof(IFileClient));

    public record InnerRequest(
        [FromForm(Name = nameof(IFileClient.UploadFileRequest.File))] IFormFile File,
        [FromForm(Name = nameof(IFileClient.UploadFileRequest.Scope))] FileScope Scope);
    
    private static async ValueTask<ErrorOr<FileModel>> HandleAsync(
        [AsParameters] InnerRequest request,
        DataContext dataContext,
        ClaimsPrincipal principal,
        IOptions<UserOptions> userOptions,
        IFileContentStorage fileContentStorage,
        CancellationToken ct)
    {
        var userId = principal.GetRequiredUserId();
        await using var contentStream = request.File.OpenReadStream();

        var hash = FileHashString.From(Convert.ToHexString(await SHA256.HashDataAsync(contentStream, ct)));
        contentStream.Seek(0, SeekOrigin.Begin);

        var identifier = FileIdentifier.From(Path.GetRandomFileName() + Path.GetExtension(request.File.FileName));

        var fileExists = await fileContentStorage.FileExists(hash, ct);
        if (fileExists is false)
        {
            var totalFileSizes = await dataContext.Files
                .Where(x => x.UploaderId == userId)
                .Select(x => new { x.Hash, x.Size })
                .Distinct()
                .SumAsync(x => x.Size, ct);
            var storageLimit = await dataContext.Users
                .Where(x => x.Id == userId)
                .Select(x => x.CustomStorageLimit)
                .FirstAsync(ct)
                ?? userOptions.Value.FileStorageLimit;

            if (totalFileSizes + contentStream.Length > storageLimit)
            {
                return Error.Forbidden("You have exceeded your file storage limit");
            }

            await fileContentStorage.SaveFile(contentStream, hash, ct);
        }

        var fileRecord = new StorageFile
        {
            Hash = hash,
            OriginalName = request.File.FileName,
            Identifier = identifier,
            Scope = request.Scope,
            Size = request.File.Length,
            CreatedAt = DateTimeOffset.UtcNow,
            UploaderId = userId,
            ContentType = request.File.ContentType
        };
        dataContext.Files.Add(fileRecord);
        await dataContext.SaveChangesAsync(ct);

        return new FileModel(
            fileRecord.Identifier,
            fileRecord.Hash,
            fileRecord.OriginalName,
            FileSize.From(fileRecord.Size),
            fileRecord.Scope);
    }
}
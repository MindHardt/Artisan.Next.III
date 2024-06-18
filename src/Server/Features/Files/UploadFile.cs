using System.Security.Claims;
using System.Security.Cryptography;
using Client.Features.Auth;
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Data;
using Server.Features.Auth;

namespace Server.Features.Files;

[Handler]
[MapPost(Contracts.UploadFile.FullPath)]
public partial class UploadFile
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint)
        => endpoint.DisableAntiforgery().RequireAuthorization().WithTags(nameof(FileEndpoints));
    
    public record Request(
        [FromForm] IFormFile File,
        [FromForm] FileScope Scope)
        : Contracts.UploadFile.Request<IFormFile>(File, Scope);

    private static async ValueTask<Results<Ok<FileModel>, ForbidHttpResult>> HandleAsync(
        [AsParameters] Request request,
        DataContext dataContext,
        ClaimsPrincipal principal,
        IOptions<UserOptions> userOptions,
        IFileStorage fileStorage,
        CancellationToken ct)
    {
        var userId = principal.GetUserId()!.Value;
        await using var contentStream = request.File.OpenReadStream();

        var hash = FileHashString.From(Convert.ToHexString(await SHA256.HashDataAsync(contentStream, ct)));
        contentStream.Seek(0, SeekOrigin.Begin);

        var identifier = FileIdentifier.From(Path.GetRandomFileName() + Path.GetExtension(request.File.FileName));

        var fileExists = await fileStorage.FileExists(hash, ct);
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
                return TypedResults.Forbid();
            }

            await fileStorage.SaveFile(contentStream, hash, ct);
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

        return TypedResults.Ok(new FileModel(
            fileRecord.Identifier,
            fileRecord.Hash,
            fileRecord.OriginalName,
            FileSize.From(fileRecord.Size),
            fileRecord.Scope));
    }
}
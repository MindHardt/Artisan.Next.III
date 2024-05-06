using System.Security.Claims;
using System.Security.Cryptography;
using Client.Features.Auth;
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Server.Data;

namespace Server.Features.Files;

[Handler]
[MapPost(Contracts.UploadFile.FullPath)]
public partial class UploadFile
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint)
        => endpoint.DisableAntiforgery().RequireAuthorization();
    
    [EndpointRegistrationOverride(nameof(AsParametersAttribute))]
    public record Request(
        [FromForm] IFormFile File,
        [FromForm] FileScope Scope)
        : Contracts.UploadFile.Request<IFormFile>(File, Scope);

    private static async ValueTask<Ok<FileModel>> HandleAsync(
        Request request,
        DataContext dataContext,
        ClaimsPrincipal principal,
        IOptions<FileStorageOptions> fsOptions,
        CancellationToken ct)
    {
        
        await using var contentStream = request.File.OpenReadStream();

        var hash = FileHashString.From(Convert.ToHexString(await SHA256.HashDataAsync(contentStream, ct)));

        var identifier = FileIdentifier.From(Path.GetRandomFileName() + Path.GetExtension(request.File.FileName));

        Directory.CreateDirectory(fsOptions.Value.Directory);
        var file = new FileInfo(Path.Combine(fsOptions.Value.Directory, hash.Value));
        if (file.Exists is false)
        {
            await using var destinationStream = file.Open(FileMode.Create);
            contentStream.Seek(0, SeekOrigin.Begin);
            await contentStream.CopyToAsync(destinationStream, ct);
        }

        var fileRecord = new StorageFile
        {
            Hash = hash,
            OriginalName = request.File.FileName,
            Identifier = identifier,
            Scope = request.Scope,
            Size = request.File.Length,
            CreatedAt = DateTimeOffset.UtcNow,
            UploaderId = principal.GetUserId()!.Value,
            ContentType = request.File.ContentType
        };
        dataContext.Files.Add(fileRecord);
        await dataContext.SaveChangesAsync(ct);

        return TypedResults.Ok(new FileModel(
            fileRecord.Identifier,
            fileRecord.Hash,
            fileRecord.OriginalName,
            fileRecord.Size,
            fileRecord.Scope));
    }
}
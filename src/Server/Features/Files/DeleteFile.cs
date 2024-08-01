using System.Security.Claims;
using Client.Features.Auth;
using Client.Features.Files;
using ErrorOr;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Infrastructure;
using FileSize = Client.Features.Files.FileSize;

namespace Server.Features.Files;

[Handler]
[MapDelete(IFileClient.DeleteFilePath)]
public partial class DeleteFile
{
    internal static Results<Ok<FileModel>, ProblemHttpResult> TransformResult(
        ErrorOr<FileModel> value) => value.GetHttpResult();
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.WithTags(nameof(IFileClient)).RequireAuthorization();

    private static async ValueTask<ErrorOr<FileModel>> HandleAsync(
        [AsParameters] IFileClient.DeleteFileRequest request,
        DataContext dataContext,
        ClaimsPrincipal principal,
        IFileContentStorage fileContentStorage,
        CancellationToken ct)
    {
        var file = await dataContext.Files
            .FirstOrDefaultAsync(x => x.Identifier == request.FileName, ct);
        if (file is null)
        {
            return Error.NotFound($"File {request.FileName} not found.");
        }

        var isUploader = file.UploaderId == principal.GetUserId();
        var isAdmin = principal.IsInRole(RoleNames.Admin);
        if (isUploader is false && isAdmin is false)
        {
            return Error.Forbidden("You cannot delete this file.");
        }

        dataContext.Files.Remove(file);
        await dataContext.SaveChangesAsync(ct);

        var hashUsed = await dataContext.Files
            .AnyAsync(x => x.Hash == file.Hash, ct);
        if (hashUsed is false)
        {
            await fileContentStorage.DeleteFile(file.Hash, ct);
        }

        return new FileModel(
            file.Identifier,
            file.Hash,
            file.OriginalName,
            FileSize.From(file.Size),
            file.Scope);
    }
}
﻿using System.Security.Claims;
using Client.Features.Auth;
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Data;

namespace Server.Features.Files;

[Handler]
[MapDelete(Contracts.DeleteFile.FullPath)]
public partial class DeleteFile
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.WithTags(nameof(FileEndpoints));

    private static async ValueTask<Results<NotFound, ForbidHttpResult, Ok<FileModel>>> HandleAsync(
        Contracts.DeleteFile.Request request,
        DataContext dataContext,
        ClaimsPrincipal principal,
        IFileStorage fileStorage,
        CancellationToken ct)
    {
        var file = await dataContext.Files
            .FirstOrDefaultAsync(x => x.Identifier == request.Identifier, ct);
        if (file is null)
        {
            return TypedResults.NotFound();
        }

        var isUploader = file.UploaderId == principal.GetUserId();
        var isAdmin = principal.IsInRole(RoleNames.Admin);
        if (isUploader is false && isAdmin is false)
        {
            return TypedResults.Forbid();
        }

        dataContext.Files.Remove(file);
        await dataContext.SaveChangesAsync(ct);

        var hashUsed = await dataContext.Files
            .AnyAsync(x => x.Hash == file.Hash, ct);
        if (hashUsed is false)
        {
            await fileStorage.DeleteFile(file.Hash, ct);
        }

        return TypedResults.Ok(new FileModel(
            file.Identifier,
            file.Hash,
            file.OriginalName,
            FileSize.From(file.Size),
            file.Scope));
    }
}
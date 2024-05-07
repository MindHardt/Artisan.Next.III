using System.Security.Claims;
using System.Text.RegularExpressions;
using Client.Features.Auth;
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Features.Files;

[Handler]
[MapGet(Contracts.SearchFiles.FullPath)]
public partial class SearchFiles
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.RequireAuthorization();
    
    private static async ValueTask<Ok<Contracts.SearchFiles.Response>> HandleAsync(
        Contracts.SearchFiles.Request request,
        ClaimsPrincipal principal,
        DataContext dataContext,
        CancellationToken ct)
    {
        var query = dataContext.Files.AsQueryable();
        if (principal.IsInRole(RoleNames.Admin) is false)
        {
            var userId = principal.GetUserId()!.Value;
            query = query.Where(x => x.UploaderId == userId);
        }
        
        if (request.Regex is not null)
        {
            // ReSharper disable once EntityFramework.UnsupportedServerSideFunctionCall
            query = query.Where(x => Regex.IsMatch(x.OriginalName, request.Regex));
        }

        if (request.RestrictedToScope is { } scope)
        {
            query = query.Where(x => x.Scope == scope);
        }

        var files = await query
            .OrderBy(x => x.CreatedAt)
            .Skip(request.PageSize * request.Page)
            .Take(request.PageSize)
            .Select(file => new FileModel(
                file.Identifier,
                file.Hash,
                file.OriginalName,
                FileSize.From(file.Size),
                file.Scope))
            .ToArrayAsync(ct);
        var totalCount = await query.CountAsync(ct);
        return TypedResults.Ok(new Contracts.SearchFiles.Response(files, totalCount));
    }
}
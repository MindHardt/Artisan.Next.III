using System.Security.Claims;
using System.Text.RegularExpressions;
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
[MapGet(IFileClient.SearchFilesPath)]
public partial class SearchFiles
{
    internal static Results<Ok<IFileClient.SearchFilesResponse>, ProblemHttpResult> TransformResult(
        ErrorOr<IFileClient.SearchFilesResponse> value) => value.GetHttpResult();
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.RequireAuthorization().WithTags(nameof(IFileClient));

    private static async ValueTask<ErrorOr<IFileClient.SearchFilesResponse>> HandleAsync(
        [AsParameters] IFileClient.SearchFilesRequest request,
        ClaimsPrincipal principal,
        DataContext dataContext,
        CancellationToken ct)
    {
        var query = dataContext.Files.AsQueryable();
        if (principal.IsInRole(RoleNames.Admin) is false)
        {
            var userId = principal.GetRequiredUserId();
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
        return new IFileClient.SearchFilesResponse(files, totalCount);
    }
}
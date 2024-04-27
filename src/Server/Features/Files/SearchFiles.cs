using System.Text.RegularExpressions;
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
    private static async ValueTask<Ok<FileModel[]>> HandleAsync(
        Contracts.SearchFiles.Request request,
        DataContext dataContext,
        CancellationToken ct)
    {
        var query = dataContext.Files.AsQueryable();
        if (request.Regex is not null)
        {
            // ReSharper disable once EntityFramework.UnsupportedServerSideFunctionCall
            query = query.Where(x => Regex.IsMatch(x.OriginalName, request.Regex));
        }

        if (request.RestrictedToScope is { } scope)
        {
            query = query.Where(x => x.Scope == scope);
        }

        return TypedResults.Ok(await query
            .OrderBy(x => x.CreatedAt)
            .Skip(request.PageSize * request.Page)
            .Take(request.PageSize)
            .Select(file => new FileModel(
                file.Identifier,
                file.Hash,
                file.OriginalName,
                file.Size))
            .ToArrayAsync(ct));
    }
}
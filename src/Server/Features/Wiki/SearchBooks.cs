using System.Text.RegularExpressions;
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Features.Wiki;

[Handler]
[MapGet(Contracts.SearchBooks.FullPath)]
public partial class SearchBooks
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.RequireAuthorization(policy => policy.RequireRole(RoleNames.Admin));

    private static async ValueTask<Ok<BookModel[]>> HandleAsync(
        Contracts.SearchBooks.Request request,
        DataContext dataContext,
        CancellationToken ct)
    {
        var query = dataContext.Books
            .OrderBy(x => x.LastUpdated)
            .AsQueryable();

        if (string.IsNullOrWhiteSpace(request.Regex) is false)
        {
            // ReSharper disable once EntityFramework.UnsupportedServerSideFunctionCall
            query = query.Where(book => Regex.IsMatch(book.Name, request.Regex));
        }

        return TypedResults.Ok(await query
            .Select(book => new BookModel(
                book.UrlName,
                book.Name,
                book.Description,
                book.ImageUrl,
                book.Author))
            .ToArrayAsync(ct));
    }
}
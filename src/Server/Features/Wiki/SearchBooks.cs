using System.Security.Claims;
using System.Text.RegularExpressions;
using Client.Features.Auth;
using Client.Features.Wiki;
using Client.Features.Wiki.Books;
using ErrorOr;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Infrastructure;

namespace Server.Features.Wiki;

[Handler]
[MapGet(IWikiClient.SearchBooksPath)]
public partial class SearchBooks
{
    internal static Results<Ok<IReadOnlyCollection<BookModel>>, ProblemHttpResult> TransformResult(
        ErrorOr<IReadOnlyCollection<BookModel>> value) => value.GetHttpResult();
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.WithTags(nameof(IWikiClient));

    private static async ValueTask<ErrorOr<IReadOnlyCollection<BookModel>>> HandleAsync(
        [AsParameters] IWikiClient.SearchBooksRequest request,
        ClaimsPrincipal principal,
        DataContext dataContext,
        CancellationToken ct)
    {
        var query = dataContext.Books
            .OrderByDescending(x => x.LastUpdated)
            .AsQueryable();

        if (principal.IsInRole(RoleNames.Admin) is false)
        {
            query = principal.GetUserId() is { } userId
                ? query.Where(book => book.IsPublic || book.Visits!.Any(visit => visit.UserId == userId))
                : query.Where(book => book.IsPublic);
        }

        if (string.IsNullOrWhiteSpace(request.Regex) is false)
        {
            // ReSharper disable once EntityFramework.UnsupportedServerSideFunctionCall
            query = query.Where(book => Regex.IsMatch(book.Name, request.Regex));
        }

        return await query
            .Select(book => new BookModel(
                book.UrlName,
                book.Name,
                book.Description,
                book.ImageUrl,
                book.Author,
                book.IsPublic))
            .ToArrayAsync(ct);
    }
}
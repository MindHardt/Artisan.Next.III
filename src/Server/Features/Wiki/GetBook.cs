using System.Security.Claims;
using Client.Features.Auth;
using Client.Features.Wiki;
using ErrorOr;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Infrastructure;

namespace Server.Features.Wiki;

[Handler]
[MapGet(IWikiClient.GetBookPath)]
public partial class GetBook
{
    internal static Results<Ok<IWikiClient.GetBookResponse>, ProblemHttpResult> TransformResult(
        ErrorOr<IWikiClient.GetBookResponse> value) => value.GetHttpResult();
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.WithTags(nameof(IWikiClient));

    private static async ValueTask<ErrorOr<IWikiClient.GetBookResponse>> HandleAsync(
        [AsParameters] IWikiClient.GetBookRequest request,
        DataContext dataContext,
        ClaimsPrincipal principal,
        CancellationToken ct)
    {
        var book = await dataContext.Books
            .FirstOrDefaultAsync(x => x.UrlName == request.UrlName, ct);
        if (book is null)
        {
            return Error.NotFound($"Book {request.UrlName} not found.");
        }

        var canView = book.IsPublic || await CheckCanView(request, dataContext, principal, ct);
        if (canView is false)
        {
            return Error.Forbidden($"You cannot view book {request.UrlName}");
        }

        return new IWikiClient.GetBookResponse(
            book.UrlName,
            book.Name,
            book.Description,
            book.Author,
            book.ImageUrl,
            book.IsPublic,
            book.Text);
    }

    private static async ValueTask<bool> CheckCanView(
        [AsParameters] IWikiClient.GetBookRequest request,
        DataContext dataContext,
        ClaimsPrincipal principal,
        CancellationToken ct)
    {
        if (principal.IsInRole(RoleNames.Admin))
        {
            return true;
        }

        var inviteKeyExists =
            request.InviteKey is not null &&
            await dataContext.BookInvites.AnyAsync(x =>
                x.BookName == request.UrlName && x.Key == request.InviteKey && x.Status == BookInviteStatus.Active, ct);

        if (principal.Identity?.IsAuthenticated is not true)
        {
            return inviteKeyExists;
        }

        var userId = principal.GetUserId()!.Value;
        var visitExists = await dataContext.BookVisits.AnyAsync(x =>
            x.UserId == userId && x.BookName == request.UrlName, ct);
        if (visitExists)
        {
            return true;
        }

        if (inviteKeyExists is false)
        {
            return false;
        }

        dataContext.BookVisits.Add(new BookVisit
        {
            BookName = request.UrlName,
            UserId = userId
        });
        await dataContext.SaveChangesAsync(ct);
        return true;
    }
}
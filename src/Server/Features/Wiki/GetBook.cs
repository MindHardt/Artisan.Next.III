using System.Security.Claims;
using Client.Features.Auth;
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Features.Wiki;

[Handler]
[MapGet(Contracts.GetBook.FullPath)]
public partial class GetBook
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.WithTags(nameof(WikiEndpoints));

    private static async ValueTask<Results<Ok<Contracts.GetBook.Response>, NotFound, ForbidHttpResult>> HandleAsync(
        Contracts.GetBook.Request request,
        DataContext dataContext,
        ClaimsPrincipal principal,
        CancellationToken ct)
    {
        var book = await dataContext.Books
            .FirstOrDefaultAsync(x => x.UrlName == request.UrlName, ct);
        if (book is null)
        {
            return TypedResults.NotFound();
        }

        var canView = book.IsPublic || await CheckCanView(request, dataContext, principal, ct);
        if (canView is false)
        {
            return TypedResults.Forbid();
        }

        return TypedResults.Ok(new Contracts.GetBook.Response(
            book.UrlName,
            book.Name,
            book.Description,
            book.Author,
            book.Text,
            book.ImageUrl,
            book.IsPublic));
    }

    private static async ValueTask<bool> CheckCanView(
        Contracts.GetBook.Request request,
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
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
[MapPost(IWikiClient.CreateBookInvitePath)]
public partial class CreateBookInvite
{
    internal static Results<Ok<BookInviteKey>, ProblemHttpResult> TransformResult(
        ErrorOr<BookInviteKey> value) => value.GetHttpResult();
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint
        .RequireAuthorization(policy => policy.RequireRole(RoleNames.Admin)).WithTags(nameof(IWikiClient));

    private static async ValueTask<ErrorOr<BookInviteKey>> HandleAsync(
        [AsParameters] IWikiClient.CreateBookInviteRequest request,
        DataContext dataContext,
        CancellationToken ct = default)
    {
        var book = await dataContext.Books
            .FirstOrDefaultAsync(x => x.UrlName == request.UrlName, ct);
        if (book is null)
        {
            return Error.NotFound($"Book {request.UrlName} not found");
        }

        var existingInvite = await dataContext.BookInvites.FirstOrDefaultAsync(x =>
            x.Book == book && x.Status == BookInviteStatus.Active, ct);
        if (existingInvite is not null)
        {
            return existingInvite.Key;
        }

        var invite = new BookInvite
        {
            Key =BookInviteKey.Create(),
            BookName = book.UrlName,
            Status = BookInviteStatus.Active
        };
        dataContext.BookInvites.Add(invite);
        await dataContext.SaveChangesAsync(ct);

        return invite.Key;
    }
}
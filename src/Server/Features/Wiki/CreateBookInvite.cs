using System.Collections.Immutable;
using System.Security.Claims;
using System.Security.Cryptography;
using Client.Features.Auth;
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Features.Wiki;

[Handler]
[MapPost(Contracts.CreateBookInvite.FullPath)]
public partial class CreateBookInvite
{
    private static readonly ImmutableArray<char> KeyChars = [..BookInviteKey.AllowedSymbols];

    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint
        .RequireAuthorization(policy => policy.RequireRole(RoleNames.Writer)).WithTags(nameof(WikiEndpoints));

    private static async ValueTask<Results<Ok<BookInviteKey>, NotFound, ForbidHttpResult>> HandleAsync(
        [AsParameters] Contracts.CreateBookInvite.Request request,
        ClaimsPrincipal principal,
        DataContext dataContext,
        CancellationToken ct = default)
    {
        var book = await dataContext.Books
            .FirstOrDefaultAsync(x => x.UrlName == request.UrlName, ct);
        if (book is null)
        {
            return TypedResults.NotFound();
        }

        var canAddInvite = principal.GetUserId() is { } userId && book.OwnerId == userId ||
                           principal.IsInRole(RoleNames.Admin);
        if (canAddInvite is false)
        {
            return TypedResults.Forbid();
        }

        var existingInvite = await dataContext.BookInvites.FirstOrDefaultAsync(x =>
            x.Book == book && x.Status == BookInviteStatus.Active, ct);
        if (existingInvite is not null)
        {
            return TypedResults.Ok(existingInvite.Key);
        }

        var invite = new BookInvite
        {
            Key = CreateKey(),
            BookName = book.UrlName,
            Status = BookInviteStatus.Active
        };
        dataContext.BookInvites.Add(invite);
        await dataContext.SaveChangesAsync(ct);

        return TypedResults.Ok(invite.Key);
    }

    private static BookInviteKey CreateKey() =>
        BookInviteKey.From(RandomNumberGenerator.GetString(KeyChars.AsSpan(), BookInviteKey.MaxLength));
}
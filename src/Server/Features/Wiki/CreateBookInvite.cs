using System.Collections.Immutable;
using System.Security.Cryptography;
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
        .RequireAuthorization(policy => policy.RequireRole(RoleNames.Admin)).WithTags(nameof(WikiEndpoints));

    [EndpointRegistrationOverride(nameof(AsParametersAttribute))]
    public record Request(
        [FromRoute] BookUrlName UrlName) 
        : Contracts.CreateBookInvite.Request(UrlName);
    
    private static async ValueTask<Results<Ok<BookInviteKey>, NotFound>> HandleAsync(
        Request request,
        DataContext dataContext,
        CancellationToken ct = default)
    {
        var book = await dataContext.Books
            .FirstOrDefaultAsync(x => x.UrlName == request.UrlName, ct);
        if (book is null)
        {
            return TypedResults.NotFound();
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
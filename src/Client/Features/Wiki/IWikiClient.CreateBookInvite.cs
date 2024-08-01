using Client.Features.Wiki.Books;
using ErrorOr;
using Refit;

namespace Client.Features.Wiki;

public partial interface IWikiClient
{
    public const string CreateBookInvitePath = $"{Prefix}/book-invites";
    [Post(CreateBookInvitePath)]
    public Task<ErrorOr<BookInviteKey>> CreateInvite(
        [Query] CreateBookInviteRequest request, CancellationToken ct = default);
    public record CreateBookInviteRequest(
        BookUrlName UrlName);
}
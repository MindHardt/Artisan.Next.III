using Client.Features.Wiki.Books;
using ErrorOr;
using Refit;

namespace Client.Features.Wiki;

public partial interface IWikiClient
{
    public const string GetBookPath = $"{Prefix}/books";

    [Get(GetBookPath)]
    public Task<ErrorOr<GetBookResponse>> GetBook(
        [Query] GetBookRequest request, CancellationToken ct = default);
    
    public record GetBookRequest(
        BookUrlName UrlName, 
        BookInviteKey? InviteKey = null);
    public record GetBookResponse(
        BookUrlName UrlName,
        string Name,
        string Description,
        string Author,
        string? ImageUrl,
        bool IsPublic,
        string Text)
        : BookModel(UrlName, Name, Description, ImageUrl, Author, IsPublic);
}
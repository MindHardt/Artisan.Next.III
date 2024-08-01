using Client.Features.Wiki.Books;
using ErrorOr;
using Refit;

namespace Client.Features.Wiki;

public partial interface IWikiClient
{
    public const string UpdateBookPath = $"{Prefix}/books";
    [Put(UpdateBookPath)]
    public Task<ErrorOr<BookModel>> UpdateBook(
        [Body] UpdateBookRequest request, CancellationToken ct = default);
    public record UpdateBookRequest(
        BookUrlName UrlName,
        string Author,
        string Description,
        string Text,
        string? ImageUrl,
        bool IsPublic);
}
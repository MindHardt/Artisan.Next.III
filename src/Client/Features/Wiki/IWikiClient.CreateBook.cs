using Client.Features.Wiki.Books;
using ErrorOr;
using Refit;

namespace Client.Features.Wiki;

public partial interface IWikiClient
{
    public const string CreateBookPath = $"{Prefix}/books";
    [Post(CreateBookPath)]
    public Task<ErrorOr<BookModel>> CreateBook(
        [Body] CreateBookRequest request, CancellationToken ct = default);
    public record CreateBookRequest(
        string Name,
        string Description,
        string Author,
        string Text,
        string? ImageUrl,
        bool IsPublic);
}
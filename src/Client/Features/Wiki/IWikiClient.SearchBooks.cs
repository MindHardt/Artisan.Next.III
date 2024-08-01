using Client.Features.Wiki.Books;
using ErrorOr;
using Refit;

namespace Client.Features.Wiki;

public partial interface IWikiClient
{
    public const string SearchBooksPath = $"{Prefix}/books/search";
    [Get(SearchBooksPath)]
    public Task<ErrorOr<IReadOnlyCollection<BookModel>>> SearchBooks(
        [Query] SearchBooksRequest request, CancellationToken ct = default);
    public record SearchBooksRequest(string? Regex = null, int Limit = 30);
}
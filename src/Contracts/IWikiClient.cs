namespace Contracts;

public interface IWikiClient
{
    public Task<BookModel> CreateBook(CreateBook.Request request, CancellationToken ct = default);
    public Task<BookModel> UpdateBook(UpdateBook.Request request, CancellationToken ct = default);
    public Task<GetBook.Response> GetBook(GetBook.Request request, CancellationToken ct = default);
    public Task<BookModel[]> SearchBooks(SearchBooks.Request request, CancellationToken ct = default);
}
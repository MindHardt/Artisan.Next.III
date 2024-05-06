using ErrorOr;

namespace Contracts;

public interface IWikiClient
{
    public Task<ErrorOr<BookModel>> CreateBook(CreateBook.Request request, CancellationToken ct = default);
    public Task<ErrorOr<BookModel>> UpdateBook(UpdateBook.Request request, CancellationToken ct = default);
    public Task<ErrorOr<GetBook.Response>> GetBook(GetBook.Request request, CancellationToken ct = default);
    public Task<ErrorOr<BookModel[]>> SearchBooks(SearchBooks.Request request, CancellationToken ct = default);
}
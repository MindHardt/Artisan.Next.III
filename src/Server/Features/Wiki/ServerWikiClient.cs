using Client.Features.Wiki;
using Client.Features.Wiki.Books;
using ErrorOr;

namespace Server.Features.Wiki;

[RegisterScoped]
public class ServerWikiClient(IServiceProvider sp) : IWikiClient
{
    public async Task<ErrorOr<IReadOnlyCollection<BookModel>>> SearchBooks(IWikiClient.SearchBooksRequest request, CancellationToken ct = default)
        => await sp.GetRequiredService<SearchBooks.Handler>().HandleAsync(request, ct);
    
    public async Task<ErrorOr<IWikiClient.GetBookResponse>> GetBook(IWikiClient.GetBookRequest request, CancellationToken ct = default)
        => await sp.GetRequiredService<GetBook.Handler>().HandleAsync(request, ct);
    
    public Task<ErrorOr<BookModel>> UpdateBook(IWikiClient.UpdateBookRequest request, CancellationToken ct = default)
        => throw new NotSupportedException();

    public Task<ErrorOr<BookInviteKey>> CreateInvite(IWikiClient.CreateBookInviteRequest request, CancellationToken ct = default)
        => throw new NotSupportedException();

    public Task<ErrorOr<BookModel>> CreateBook(IWikiClient.CreateBookRequest request, CancellationToken ct = default)
        => throw new NotSupportedException();
}
using Contracts;
using ErrorOr;
using Server.Features.Shared;

namespace Server.Features.Wiki;

[RegisterScoped]
public class WikiClient(IServiceProvider sp) : IWikiClient
{
    public async Task<ErrorOr<BookModel>> CreateBook(Contracts.CreateBook.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<CreateBook.Handler>();
        return (await handler.HandleAsync(request, ct)).AsErrorOr<BookModel>();
    }

    public async Task<ErrorOr<BookModel>> UpdateBook(Contracts.UpdateBook.Request request, CancellationToken ct = default)
    {
        var innerRequest = new UpdateBook.Request(request.UrlName, request);

        var handler = sp.GetRequiredService<UpdateBook.Handler>();
        return (await handler.HandleAsync(innerRequest, ct)).AsErrorOr<BookModel>();
    }

    public async Task<ErrorOr<Contracts.GetBook.Response>> GetBook(Contracts.GetBook.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<GetBook.Handler>();
        return (await handler.HandleAsync(request, ct)).AsErrorOr<Contracts.GetBook.Response>();
    }

    public async Task<ErrorOr<BookModel[]>> SearchBooks(Contracts.SearchBooks.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<SearchBooks.Handler>();
        return (await handler.HandleAsync(request, ct)).AsErrorOr<BookModel[]>();
    }

    public async Task<ErrorOr<BookInviteKey>> CreateInviteKey(Contracts.CreateBookInvite.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<CreateBookInvite.Handler>();
        return (await handler.HandleAsync(request, ct)).AsErrorOr<BookInviteKey>();
    }
}
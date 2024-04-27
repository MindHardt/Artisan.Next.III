using Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Server.Features.Wiki;

[RegisterScoped]
public class WikiClient(IServiceProvider sp) : IWikiClient
{
    public async Task<BookModel> CreateBook(Contracts.CreateBook.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<CreateBook.Handler>();
        var result = await handler.HandleAsync(request, ct);

        return ((Ok<BookModel>)result.Result).Value!;
    }

    public async Task<BookModel> UpdateBook(Contracts.UpdateBook.Request request, CancellationToken ct = default)
    {
        var innerRequest = new UpdateBook.Request(request.UrlName, request);
        
        var handler = sp.GetRequiredService<UpdateBook.Handler>();
        var result = await handler.HandleAsync(innerRequest, ct);

        return ((Ok<BookModel>)result.Result).Value!;
    }

    public async Task<Contracts.GetBook.Response> GetBook(Contracts.GetBook.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<GetBook.Handler>();
        var result = await handler.HandleAsync(request, ct);
        
        return ((Ok<Contracts.GetBook.Response>)result.Result).Value!;
    }

    public async Task<BookModel[]> SearchBooks(Contracts.SearchBooks.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<SearchBooks.Handler>();
        var result = await handler.HandleAsync(request, ct);

        return ((Ok<BookModel[]>)result).Value!;
    }
}
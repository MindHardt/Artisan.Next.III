using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using Client.Features.Shared;
using Contracts;
using ErrorOr;
using Microsoft.Extensions.Options;

namespace Client.Features.Wiki;

[RegisterScoped]
public class WikiClient(HttpClient http, IOptions<JsonSerializerOptions> jsonOptions) : IWikiClient
{
    public async Task<ErrorOr<BookModel>> CreateBook(CreateBook.Request request, CancellationToken ct = default)
    {
        return await http.PostAsJsonAsync(Contracts.CreateBook.FullPath, request, ct)
            .AsErrorOr<BookModel>(jsonOptions.Value, ct);
    }

    public async Task<ErrorOr<BookModel>> UpdateBook(UpdateBook.Request request, CancellationToken ct = default)
    {
        var url = Contracts.UpdateBook.FullPath
            .Replace($"{{{nameof(request.UrlName)}}}", request.UrlName.Value);

        return await http.PutAsJsonAsync(url, request, ct)
            .AsErrorOr<BookModel>(jsonOptions.Value, ct);
    }

    public async Task<ErrorOr<GetBook.Response>> GetBook(GetBook.Request request, CancellationToken ct = default)
    {
        var url = Contracts.GetBook.FullPath
            .Replace($"{{{nameof(request.UrlName)}}}", request.UrlName.Value);

        return await http.GetAsync(url, ct)
            .AsErrorOr<GetBook.Response>(jsonOptions.Value, ct);
    }

    public async Task<ErrorOr<BookModel[]>> SearchBooks(SearchBooks.Request request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Regex))
        {
            return await http.GetAsync(Contracts.SearchBooks.FullPath, ct)
                .AsErrorOr<BookModel[]>(jsonOptions.Value, ct);
        }

        var query = HttpUtility.ParseQueryString(string.Empty);
        query[nameof(request.Regex)] = request.Regex;

        return await http.GetAsync($"{Contracts.SearchBooks.FullPath}?{query}", ct)
            .AsErrorOr<BookModel[]>(jsonOptions.Value, ct);
    }

    public async Task<ErrorOr<BookInviteKey>> CreateInviteKey(CreateBookInvite.Request request, CancellationToken ct = default)
    {
        var url = CreateBookInvite.FullPath
            .Replace($"{{{nameof(request.UrlName)}}}", request.UrlName.Value);

        return await http.PostAsync(url, null, ct)
            .AsErrorOr<BookInviteKey>(jsonOptions.Value, ct);
    }
}
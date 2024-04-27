using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using Contracts;
using Microsoft.Extensions.Options;

namespace Client.Features.Wiki;

[RegisterScoped]
public class WikiClient(HttpClient http, IOptions<JsonSerializerOptions> jsonOptions) : IWikiClient
{
    public async Task<BookModel> CreateBook(CreateBook.Request request, CancellationToken ct = default)
    {
        var response = await http.PostAsJsonAsync(Contracts.CreateBook.FullPath, request, ct);
        response.EnsureSuccessStatusCode();
        
        return (await response.Content.ReadFromJsonAsync<BookModel>(ct))!;
    }

    public async Task<BookModel> UpdateBook(UpdateBook.Request request, CancellationToken ct = default)
    {
        var url = Contracts.UpdateBook.FullPath
            .Replace($"{{{nameof(request.UrlName)}}}", request.UrlName.Value);

        var response = await http.PutAsJsonAsync(url, request, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<BookModel>(jsonOptions.Value, ct))!;
    }

    public async Task<GetBook.Response> GetBook(GetBook.Request request, CancellationToken ct = default)
    {
        var url = Contracts.GetBook.FullPath
            .Replace($"{{{nameof(request.UrlName)}}}", request.UrlName.Value);

        return (await http.GetFromJsonAsync<GetBook.Response>(url, jsonOptions.Value, ct))!;
    }

    public async Task<BookModel[]> SearchBooks(SearchBooks.Request request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Regex))
        {
            return (await http.GetFromJsonAsync<BookModel[]>(Contracts.SearchBooks.FullPath, jsonOptions.Value, ct))!;
        }
        
        var query = HttpUtility.ParseQueryString(string.Empty);
        query[nameof(request.Regex)] = request.Regex;

        return (await http.GetFromJsonAsync<BookModel[]>($"{Contracts.SearchBooks.FullPath}?{query}", jsonOptions.Value, ct))!;
    }
}
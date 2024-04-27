using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using Contracts;
using Microsoft.Extensions.Options;

namespace Client.Features.Files;

[RegisterScoped]
public class FileClient(HttpClient http, IOptions<JsonSerializerOptions> jsonOptions) : IFileClient
{
    public async Task<FileModel> UploadFile(UploadFile.Request<UploadFile.PostedFile> request, CancellationToken ct = default)
    {
        using var form = new MultipartFormDataContent();
        form.Add(new StreamContent(request.File.Content)
        {
            Headers = { ContentType = new MediaTypeHeaderValue(request.File.ContentType) }
        }, nameof(request.File), request.File.FileName);
        form.Add(new StringContent(request.Scope.ToString()), nameof(request.Scope));

        var response = await http.PostAsync(Contracts.UploadFile.FullPath, form, ct);
        return (await response.Content.ReadFromJsonAsync<FileModel>(ct))!;
    }

    public async Task<FileModel[]> SearchFiles(SearchFiles.Request request, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query[nameof(request.Page)] = request.Page.ToString();
        query[nameof(request.PageSize)] = request.PageSize.ToString();
        
        if (request.Regex is not null)
        {
            query[nameof(request.Regex)] = request.Regex;
        }

        if (request.RestrictedToScope is { } scope)
        {
            query[nameof(request.RestrictedToScope)] = scope.ToString();
        }

        return (await http.GetFromJsonAsync<FileModel[]>($"{Contracts.SearchFiles.FullPath}?{query}", jsonOptions.Value, ct))!;
    }

    public async Task<FileModel> DeleteFile(DeleteFile.Request request, CancellationToken ct = default)
    {
        var url = Contracts.DeleteFile.FullPath.Replace($"{{{nameof(request.Identifier)}}}", request.Identifier.Value);

        return (await http.DeleteFromJsonAsync<FileModel>(url, jsonOptions.Value, ct))!;
    }
}
﻿using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;
using Client.Features.Shared;
using Contracts;
using ErrorOr;
using Microsoft.Extensions.Options;

namespace Client.Features.Files;

[RegisterScoped]
public class FileClient(HttpClient http, IOptions<JsonSerializerOptions> jsonOptions) : IFileClient
{
    public async Task<ErrorOr<FileModel>> UploadFile(UploadFile.Request<UploadFile.PostedFile> request, CancellationToken ct = default)
    {
        using var form = new MultipartFormDataContent();
        form.Add(new StreamContent(request.File.Content)
        {
            Headers =
            {
                ContentType = string.IsNullOrEmpty(request.File.ContentType)
                    ? null
                    : new MediaTypeHeaderValue(request.File.ContentType)
            }
        }, nameof(request.File), request.File.FileName);

        form.Add(new StringContent(request.Scope.ToString()), nameof(request.Scope));

        return await http.PostAsync(Contracts.UploadFile.FullPath, form, ct)
            .AsErrorOr<FileModel>(jsonOptions.Value, ct);
    }

    public async Task<ErrorOr<SearchFiles.Response>> SearchFiles(SearchFiles.Request request, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query[nameof(request.Page)] = request.Page.ToString();
        query[nameof(request.PageSize)] = request.PageSize.ToString();

        if (string.IsNullOrEmpty(request.Regex) is false)
        {
            query[nameof(request.Regex)] = request.Regex;
        }

        if (request.RestrictedToScope is { } scope)
        {
            query[nameof(request.RestrictedToScope)] = scope.ToString();
        }

        return await http.GetAsync($"{Contracts.SearchFiles.FullPath}?{query}", ct)
            .AsErrorOr<SearchFiles.Response>(jsonOptions.Value, ct);
    }

    public async Task<ErrorOr<FileModel>> DeleteFile(DeleteFile.Request request, CancellationToken ct = default)
    {
        var url = Contracts.DeleteFile.FullPath.Replace($"{{{nameof(request.Identifier)}}}", request.Identifier.Value);

        return await http.DeleteAsync(url, ct).AsErrorOr<FileModel>(jsonOptions.Value, ct);
    }

    public async Task<ErrorOr<GetFileUsage.Response>> GetFileUsage(CancellationToken ct = default)
    {
        return await http.GetAsync(Contracts.GetFileUsage.FullPath, ct).AsErrorOr<GetFileUsage.Response>(jsonOptions.Value, ct);
    }

    public async Task<ErrorOr<IReadOnlyCollection<FileIdentifier>>> GetBlogposts(CancellationToken ct = default)
    {
        return await http.GetAsync(Contracts.GetBlogposts.FullPath, ct).AsErrorOr<IReadOnlyCollection<FileIdentifier>>(jsonOptions.Value, ct);
    }
}
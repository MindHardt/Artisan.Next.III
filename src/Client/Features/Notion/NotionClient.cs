﻿using System.Text.Json;
using System.Web;
using Client.Features.Shared;
using Contracts;
using ErrorOr;
using Microsoft.Extensions.Options;

namespace Client.Features.Notion;

[RegisterScoped]
public class NotionClient(HttpClient http, IOptions<JsonSerializerOptions> json) : INotionClient
{
    public async Task<ErrorOr<GetStatusEffects.Model[]>> GetStatusEffects(GetStatusEffects.Request request, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (request.PartialName is not null)
        {
            query[nameof(request.PartialName)] = request.PartialName;
        }

        var uri = Contracts.GetStatusEffects.FullPath;
        if (query.HasKeys())
        {
            uri += $"?{query}";
        }

        return await (await http.GetAsync(uri, ct))
            .AsErrorOr<GetStatusEffects.Model[]>(json.Value, ct);
    }
}
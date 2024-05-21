﻿using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Notion.Client;

namespace Server.Features.Notion;

[Handler]
[MapGet(Contracts.GetStatusEffects.FullPath)]
public partial class GetStatusEffects
{
    [EndpointRegistrationOverride(nameof(AsParametersAttribute))]
    public record Request(
        string? PartialName)
        : Contracts.GetStatusEffects.Request(PartialName);
    
    private static async ValueTask<Ok<Contracts.GetStatusEffects.Model[]>> HandleAsync(
        Request request,
        IOptions<NotionConfiguration> configuration,
        INotionClient notion,
        CancellationToken ct)
    {
        var statusEffects = configuration.Value.StatusEffects;
        var filter = request.PartialName is not null
            ? new TitleFilter(statusEffects.Title, contains: request.PartialName)
            : null;
        
        var pages = await notion.Databases.QueryAsync(statusEffects.DatabaseId,
            new DatabasesQueryParameters
            {
                Filter = filter
            }, ct);

        return TypedResults.Ok(pages.Results
            .Select(x => new Contracts.GetStatusEffects.Model(
                null,
                x.GetTitle().Title.ToPlainText(),
                ((EmojiObject)x.Icon).Emoji,
                ((RichTextPropertyValue)x.Properties[statusEffects.Description]).RichText.ToHtml().Value))
            .ToArray());
    }
}
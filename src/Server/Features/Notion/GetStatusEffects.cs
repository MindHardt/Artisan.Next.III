using Client.Features.Notion;
using ErrorOr;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Notion.Client;
using Server.Infrastructure;
using INotionApiClient = Notion.Client.INotionClient;
using INotionClient = Client.Features.Notion.INotionClient;

namespace Server.Features.Notion;

[Handler]
[MapGet(INotionClient.GetStatusEffectsPath)]
public partial class GetStatusEffects
{
    internal static Results<Ok<IReadOnlyCollection<StatusEffectModel>>, ProblemHttpResult> TransformResult(
        ErrorOr<IReadOnlyCollection<StatusEffectModel>> value) => value.GetHttpResult();
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.WithTags(nameof(INotionClient));

    private static async ValueTask<ErrorOr<IReadOnlyCollection<StatusEffectModel>>> HandleAsync(
        [AsParameters] INotionClient.GetStatusEffectsRequest request,
        IOptions<NotionConfiguration> configuration,
        INotionApiClient notionApi,
        CancellationToken ct)
    {
        var statusEffects = configuration.Value.StatusEffects;
        var filter = request.PartialName is not null
            ? new TitleFilter(statusEffects.Title, contains: request.PartialName)
            : null;

        var pages = await notionApi.Databases.QueryAsync(statusEffects.DatabaseId,
            new DatabasesQueryParameters
            {
                Filter = filter
            }, ct);

        return pages.Results
            .Select(x => new StatusEffectModel(
                (x.Cover as ExternalFile)?.External.Url,
                x.GetTitle().Title.ToPlainText(),
                ((EmojiObject)x.Icon).Emoji,
                ((RichTextPropertyValue)x.Properties[statusEffects.Description]).RichText.ToHtml().Value,
                x.PublicUrl))
            .ToArray();
    }
}
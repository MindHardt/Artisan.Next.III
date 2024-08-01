using Refit;
using ErrorOr;

namespace Client.Features.Notion;

public partial interface INotionClient
{
    public const string GetStatusEffectsPath = $"{Prefix}/status-effects";
    [Get(GetStatusEffectsPath)]
    public Task<ErrorOr<IReadOnlyCollection<StatusEffectModel>>> GetStatusEffects(
        [Query] GetStatusEffectsRequest request, CancellationToken ct = default);
    public record GetStatusEffectsRequest(
        string? PartialName);
}
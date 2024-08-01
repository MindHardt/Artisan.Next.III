using Client.Features.Notion;
using ErrorOr;

namespace Server.Features.Notion;

[RegisterScoped]
public class ServerNotionClient(IServiceProvider sp) : INotionClient
{
    public async Task<ErrorOr<IReadOnlyCollection<StatusEffectModel>>> GetStatusEffects(INotionClient.GetStatusEffectsRequest request, CancellationToken ct = default)
        => await sp.GetRequiredService<GetStatusEffects.Handler>().HandleAsync(request, ct);
}
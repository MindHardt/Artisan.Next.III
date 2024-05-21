using ErrorOr;
using Server.Features.Shared;
using INotionClient = Contracts.INotionClient;

namespace Server.Features.Notion;

[RegisterScoped]
public class NotionClient(IServiceProvider sp) : INotionClient
{
    public async Task<ErrorOr<Contracts.GetStatusEffects.Model[]>> GetStatusEffects(Contracts.GetStatusEffects.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<GetStatusEffects.Handler>();
        var innerRequest = new GetStatusEffects.Request(request.PartialName);
        return (await handler.HandleAsync(innerRequest, ct)).AsErrorOr<Contracts.GetStatusEffects.Model[]>();
    }
}
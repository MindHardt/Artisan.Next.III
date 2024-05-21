using ErrorOr;

namespace Contracts;

public interface INotionClient
{
    public Task<ErrorOr<GetStatusEffects.Model[]>> GetStatusEffects(GetStatusEffects.Request request, CancellationToken ct = default);
}
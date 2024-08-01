using ErrorOr;
using Refit;

namespace Client.Features.Files;

public partial interface IFileClient
{
    public const string GetFileUsagePath = $"{Prefix}/usage";
    [Get(GetFileUsagePath)]
    public Task<ErrorOr<GetFileUsageResponse>> GetFileUsage(CancellationToken ct = default);
    public record GetFileUsageResponse(
        FileSize Used, 
        FileSize Limit);

}
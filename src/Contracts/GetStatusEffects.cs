using Arklens.Alids;

namespace Contracts;

public static class GetStatusEffects
{
    public const string Path = "status-effects";
    public const string FullPath = $"{NotionEndpoints.FullPath}/{Path}";

    public record Request;

    public record Model(
        Alid Alid,
        string? CoverUrl,
        string Name,
        string Icon,
        string Description,
        string? PageUrl);
}
namespace Contracts;

public static class GetStatusEffects
{
    public const string Path = "status-effects";
    public const string FullPath = $"{NotionEndpoints.FullPath}/{Path}";
    
    public record Request(string? PartialName);

    public record Model(
        string? CoverUrl,
        string Name,
        string Icon,
        string Description);
}
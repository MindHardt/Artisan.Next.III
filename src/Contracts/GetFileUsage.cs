namespace Contracts;

public static class GetFileUsage
{
    public const string Path = "usage";
    public const string FullPath = $"{FileEndpoints.FullPath}/{Path}";

    public record Response(FileSize Used, FileSize Limit);
}
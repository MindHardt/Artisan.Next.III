namespace Contracts;

public static class SearchFiles
{
    public const string Path = "search";
    public const string FullPath = $"{FileEndpoints.FullPath}/{Path}";

    public record Request(
        string? Regex = null,
        FileScope? RestrictedToScope = null,
        int Page = 0,
        int PageSize = 10);

    public record Response(
        FileModel[] Files,
        int TotalFiles);
}
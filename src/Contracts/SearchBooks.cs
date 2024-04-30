namespace Contracts;

public static class SearchBooks
{
    public const string Path = "books/search";
    public const string FullPath = $"{WikiEndpoints.FullPath}/{Path}";

    public record Request(string? Regex = null, int Limit = 30);
}
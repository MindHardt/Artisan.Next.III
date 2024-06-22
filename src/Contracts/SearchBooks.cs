namespace Contracts;

public static class SearchBooks
{
    public const string FullPath = BookEndpoints.FullPath;

    public record Request(string? Regex = null, int Limit = 30);
}
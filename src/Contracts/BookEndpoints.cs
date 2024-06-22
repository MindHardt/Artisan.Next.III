namespace Contracts;

public static class BookEndpoints
{
    public const string Path = "books";
    public const string FullPath = $"{WikiEndpoints.FullPath}/{Path}";
}
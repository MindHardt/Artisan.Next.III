namespace Contracts;

public static class CreateBook
{
    public const string Path = "books";
    public const string FullPath = $"{WikiEndpoints.FullPath}/{Path}";

    public record Request(
        string Name,
        string Description,
        string Author,
        string Text,
        string? ImageUrl);
}
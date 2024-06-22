namespace Contracts;

public static class CreateBook
{
    public const string FullPath = BookEndpoints.FullPath;

    public record Request(
        string Name,
        string Description,
        string Author,
        string Text,
        string? ImageUrl,
        bool IsPublic);
}
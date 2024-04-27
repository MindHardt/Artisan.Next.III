namespace Contracts;

public class GetBook
{
    public const string Path = $"books/{{{nameof(Request.UrlName)}}}";
    public const string FullPath = $"{WikiEndpoints.FullPath}/{Path}";

    public record Request(BookUrlName UrlName);

    public record Response(
        BookUrlName UrlName,
        string Name,
        string Description,
        string Author,
        string Text,
        string? ImageUrl)
        : BookModel(UrlName, Name, Description, ImageUrl, Author);
}
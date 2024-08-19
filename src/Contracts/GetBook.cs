namespace Contracts;

public class GetBook
{
    public const string Path = $"{{{nameof(Request.UrlName)}}}";
    public const string FullPath = $"{BookEndpoints.FullPath}/{Path}";

    public record Request(BookUrlName UrlName, BookInviteKey? InviteKey = null);

    public record Response(
        BookUrlName UrlName,
        string Name,
        string Description,
        string Author,
        string Text,
        string? ImageUrl,
        bool IsPublic,
        bool IsEditable)
        : BookModel(UrlName, Name, Description, ImageUrl, Author, IsPublic, IsEditable);
}
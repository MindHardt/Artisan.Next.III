namespace Client.Features.Wiki.Books;

public record BookModel(
    BookUrlName UrlName,
    string Name,
    string Description,
    string? ImageUrl,
    string Author,
    bool IsPublic);
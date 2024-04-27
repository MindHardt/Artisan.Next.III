using System.Text.Json.Serialization;

namespace Contracts;

public static class UpdateBook
{
    public const string Path = $"books/{{{nameof(Request.UrlName)}}}";
    public const string FullPath = $"{WikiEndpoints.FullPath}/{Path}";
    
    public record Request(
        [property: JsonIgnore] BookUrlName UrlName,
        string Author,
        string Description,
        string Text,
        string? ImageUrl);
}
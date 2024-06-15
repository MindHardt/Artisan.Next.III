namespace Contracts;

public static class CreateBookInvite
{
    public const string Path = $"books/{{{nameof(Request.UrlName)}}}/create-invite";
    public const string FullPath = $"{WikiEndpoints.FullPath}/{Path}";

    public record Request(BookUrlName UrlName);
}
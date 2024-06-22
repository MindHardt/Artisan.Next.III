namespace Contracts;

public static class CreateBookInvite
{
    public const string Path = $"{{{nameof(Request.UrlName)}}}/create-invite";
    public const string FullPath = $"{BookEndpoints.FullPath}/{Path}";

    public record Request(BookUrlName UrlName);
}
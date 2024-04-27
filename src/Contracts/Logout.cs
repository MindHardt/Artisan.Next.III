namespace Contracts;

public static class Logout
{
    public const string Path = "logout";
    public const string FullPath = $"{AuthEndpoints.FullPath}/{Path}";

    public record Request(string ReturnUrl);
}
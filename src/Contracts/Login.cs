namespace Contracts;

public static class Login
{
    public const string Path = $"login/{{{nameof(Request.Scheme)}}}";
    public const string FullPath = $"{AuthEndpoints.FullPath}/{Path}";

    public record Request(string Scheme, string ReturnUrl);
}
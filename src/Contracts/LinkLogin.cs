namespace Contracts;

public class LinkLogin
{
    public const string Path = $"link-login/{{{nameof(Request.Scheme)}}}";
    public const string FullPath = $"{AuthEndpoints.FullPath}/{Path}";

    public record Request(
        string Scheme,
        string ReturnUrl)
        : Login.Request(Scheme, ReturnUrl);
}
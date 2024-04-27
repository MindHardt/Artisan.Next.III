namespace Contracts;

public class AuthEndpoints
{
    public const string Path = "auth";
    public const string FullPath = $"{Api.Prefix}/{Path}";
}

public record LoginSchemeModel(string Name, string? DisplayName)
{
    public string DisplayName { get; } = DisplayName ?? Name;
}
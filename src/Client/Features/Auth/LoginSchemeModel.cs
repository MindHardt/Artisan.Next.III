namespace Client.Features.Auth;

public record LoginSchemeModel(string Name, string? DisplayName)
{
    public string DisplayName { get; } = DisplayName ?? Name;
}
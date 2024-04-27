namespace Contracts;

public class UpdateProfile
{
    public const string Path = "me";
    public const string FullPath = $"{AuthEndpoints.FullPath}/{Path}";

    public record Request(
        string? AvatarUrl,
        string? UserName);
}
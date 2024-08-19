using System.Security.Claims;
using Contracts;

namespace Client.Features.Auth;

public static class UserExtensions
{
    public static int? GetUserId(this ClaimsPrincipal principal)
        => int.TryParse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId)
            ? userId
            : null;

    public static int GetRequiredUserId(this ClaimsPrincipal principal)
        => GetUserId(principal) ?? throw new InvalidOperationException("Cannot retrieve required user id");

    public static string? GetAvatarUrl(this ClaimsPrincipal principal)
        => principal.FindFirst(CustomClaims.AvatarUrl)?.Value;

    public static string? GetUserName(this ClaimsPrincipal principal)
        => principal.FindFirst(ClaimTypes.Name)?.Value;

    public static IReadOnlyCollection<string> GetRoles(this ClaimsPrincipal principal)
        => [..principal.FindAll(ClaimTypes.Role).Select(x => x.Value)];
}
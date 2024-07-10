using System.Security.Claims;

namespace Contracts;

public record UserModel(
    int Id,
    string UserName,
    string AvatarUrl,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> LoginSchemes)
{
    public ClaimsPrincipal AsPrincipal() => new(new ClaimsIdentity(
    [
        new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
        new Claim(ClaimTypes.Name, UserName),
        new Claim(CustomClaims.AvatarUrl, AvatarUrl),

        ..Roles.Select(x => new Claim(ClaimTypes.Role, x)),
        ..LoginSchemes.Select(x => new Claim(CustomClaims.LoginScheme, x))

    ], authenticationType: nameof(UserModel)));
}
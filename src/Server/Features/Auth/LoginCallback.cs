using System.Security.Claims;
using Client.Features.Auth;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Data;

namespace Server.Features.Auth;

[Handler]
[MapGet(Path)]
public partial class LoginCallback
{
    public const string Path = $"{IAuthClient.Prefix}/login-callback";

    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint)
        => endpoint.ExcludeFromDescription();

    public record Request(
        [FromQuery] string ReturnUrl);

    private static async ValueTask<Results<RedirectHttpResult, ProblemHttpResult>> HandleAsync(
        Request request,
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        CancellationToken ct)
    {
        var loginInfo = await signInManager.GetExternalLoginInfoAsync();
         if (loginInfo is null)
        {
            return TypedResults.Problem(new ProblemDetails
            {
                Detail = "An error occured when retrieving external login info"
            });
        }

        var user = await userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
        if (user is null)
        {
            user = new User
            {
                UserName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name),
                AvatarUrl = loginInfo.Principal.FindFirstValue(CustomClaims.AvatarUrl)!
            };
            var identityResult = await userManager.CreateAsync(user);
            if (identityResult.Succeeded is false)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Detail = string.Join("\n", identityResult.Errors.Select(x => x.Description))
                });
            }

            await userManager.AddLoginAsync(user, loginInfo);
        }

        var logins = await userManager.GetLoginsAsync(user);
        await signInManager.SignInWithClaimsAsync(user, isPersistent: true, additionalClaims:
        [
            new Claim(CustomClaims.AvatarUrl, user.AvatarUrl),
            ..logins.Select(x => new Claim(CustomClaims.LoginScheme, x.LoginProvider))
        ]);
        return TypedResults.Redirect(request.ReturnUrl);
    }
}
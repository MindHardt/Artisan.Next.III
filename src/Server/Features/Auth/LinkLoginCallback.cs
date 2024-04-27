using System.Security.Claims;
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Data;

namespace Server.Features.Auth;

[Handler]
[MapGet(FullPath)]
public partial class LinkLoginCallback
{
    public const string Path = "link-login-callback";
    public const string FullPath = $"{AuthEndpoints.FullPath}/{Path}";

    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint)
        => endpoint.ExcludeFromDescription().RequireAuthorization();
    
    public record Request(
        [FromQuery] string ReturnUrl);
    
    private static async ValueTask<Results<RedirectHttpResult, ProblemHttpResult>> HandleAsync(
        Request request,
        SignInManager<User> signInManager,
        ClaimsPrincipal principal,
        UserManager<User> userManager,
        CancellationToken ct)
    {
        var loginInfo = await signInManager.GetExternalLoginInfoAsync();
        var user = (await userManager.GetUserAsync(principal))!;
        
        await userManager.AddLoginAsync(user, loginInfo!);

        var logins = await userManager.GetLoginsAsync(user);
        await signInManager.SignInWithClaimsAsync(user, isPersistent: true, additionalClaims:
        [
            new Claim(CustomClaims.AvatarUrl, user.AvatarUrl),
            ..logins.Select(x => new Claim(CustomClaims.LoginScheme, x.LoginProvider))
        ]);
        return TypedResults.Redirect(request.ReturnUrl);
    }
}
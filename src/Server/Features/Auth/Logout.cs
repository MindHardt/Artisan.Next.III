using Client.Features.Auth;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Server.Data;

namespace Server.Features.Auth;

[Handler]
[MapGet(IAuthClient.LogoutPath)]
public partial class Logout
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.WithTags(nameof(IAuthClient));

    private static async ValueTask<RedirectHttpResult> HandleAsync(
        [AsParameters] IAuthClient.LogoutRequest request,
        SignInManager<User> signInManager,
        CancellationToken ct)
    {
        await signInManager.SignOutAsync();

        return TypedResults.Redirect(request.ReturnUrl);
    }
}
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Server.Data;

namespace Server.Features.Auth;

[Handler]
[MapGet(Contracts.Logout.FullPath)]
public partial class Logout
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.WithTags(nameof(AuthEndpoints));
    
    private static async ValueTask<RedirectHttpResult> HandleAsync(
        Contracts.Logout.Request request,
        SignInManager<User> signInManager,
        CancellationToken ct)
    {
        await signInManager.SignOutAsync();

        return TypedResults.Redirect(request.ReturnUrl);
    }
}
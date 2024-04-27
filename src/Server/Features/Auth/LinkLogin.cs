using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Server.Data;

namespace Server.Features.Auth;

[Handler]
[MapGet(Contracts.LinkLogin.FullPath)]
public partial class LinkLogin
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.RequireAuthorization();


    private static ValueTask<ChallengeHttpResult> HandleAsync(
        Contracts.LinkLogin.Request request,
        SignInManager<User> signInManager,
        HttpRequest httpRequest,
        CancellationToken ct)
    {
        var query = QueryString.Create(nameof(request.ReturnUrl), request.ReturnUrl);
        var redirectUrl = UriHelper.BuildRelative(
            httpRequest.PathBase,
            LinkLoginCallback.FullPath,
            query);

        var authProperties = signInManager.ConfigureExternalAuthenticationProperties(request.Scheme, redirectUrl);

        return ValueTask.FromResult(TypedResults.Challenge(authProperties, [request.Scheme]));
    }
}
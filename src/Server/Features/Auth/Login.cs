using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Data;

namespace Server.Features.Auth;

[Handler]
[MapGet(Contracts.Login.FullPath)]
public partial class Login
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.WithTags(nameof(AuthEndpoints));
    
    [EndpointRegistrationOverride(nameof(AsParametersAttribute))]
    public record Request(
        [FromRoute] string Scheme,
        [FromQuery] string ReturnUrl)
        : Contracts.Login.Request(Scheme, ReturnUrl);

    private static ValueTask<ChallengeHttpResult> HandleAsync(
        Request request,
        SignInManager<User> signInManager,
        HttpRequest httpRequest,
        CancellationToken ct)
    {
        var query = QueryString.Create(nameof(request.ReturnUrl), request.ReturnUrl);
        var redirectUrl = UriHelper.BuildRelative(
            httpRequest.PathBase,
            LoginCallback.FullPath,
            query);

        var authProperties = signInManager.ConfigureExternalAuthenticationProperties(request.Scheme, redirectUrl);

        return ValueTask.FromResult(TypedResults.Challenge(authProperties, [request.Scheme]));
    }
}
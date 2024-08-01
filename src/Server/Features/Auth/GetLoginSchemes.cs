using Client.Features.Auth;
using Client.Infrastructure;
using ErrorOr;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Server.Data;
using Server.Infrastructure;

namespace Server.Features.Auth;

[Handler]
[MapGet(IAuthClient.GetLoginSchemesPath)]
public partial class GetLoginSchemes
{
    internal static Results<Ok<IReadOnlyCollection<LoginSchemeModel>>, ProblemHttpResult> TransformResult(
        ErrorOr<IReadOnlyCollection<LoginSchemeModel>> value) => value.GetHttpResult();
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.WithTags(nameof(IAuthClient));

    private static async ValueTask<ErrorOr<IReadOnlyCollection<LoginSchemeModel>>> HandleAsync(
        [AsParameters] EmptyRequest _,
        SignInManager<User> signInManager,
        CancellationToken ct)
    {
        var authSchemes = await signInManager.GetExternalAuthenticationSchemesAsync();

        return authSchemes
            .Select(x => new LoginSchemeModel(x.Name, x.DisplayName))
            .ToArray();
    }
}
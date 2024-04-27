using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Server.Data;

namespace Server.Features.Auth;

[Handler]
[MapGet(Contracts.GetLoginSchemes.FullPath)]
public partial class GetLoginSchemes
{
    private static async ValueTask<Ok<LoginSchemeModel[]>> HandleAsync(
        EmptyRequest _,
        SignInManager<User> signInManager,
        CancellationToken ct)
    {
        var authSchemes = await signInManager.GetExternalAuthenticationSchemesAsync();

        return TypedResults.Ok(authSchemes
            .Select(x => new LoginSchemeModel(x.Name, x.DisplayName))
            .ToArray());
    }
}
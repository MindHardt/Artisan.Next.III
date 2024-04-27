using System.Security.Claims;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Server.Data;

namespace Server.Features.Auth;

[Handler]
[MapPut(Contracts.UpdateProfile.FullPath)]
public partial class UpdateProfile
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint
        .RequireAuthorization();

    private static async ValueTask<Ok> HandleAsync(
        Contracts.UpdateProfile.Request request,
        UserManager<User> userManager,
        ClaimsPrincipal principal,
        CancellationToken ct)
    {
        var user = (await userManager.GetUserAsync(principal))!;

        user.UserName = request.UserName ?? user.UserName;
        user.AvatarUrl = request.AvatarUrl ?? user.AvatarUrl;

        await userManager.UpdateAsync(user);

        return TypedResults.Ok();
    }
}
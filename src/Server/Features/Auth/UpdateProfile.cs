using System.Security.Claims;
using Client.Features.Auth;
using ErrorOr;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Infrastructure;

namespace Server.Features.Auth;

[Handler]
[MapPut(IAuthClient.UpdateProfilePath)]
public partial class UpdateProfile
{
    internal static Results<Ok, ProblemHttpResult> TransformResult(
        Error? value) => value.GetHttpResult();
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint
        .RequireAuthorization().WithTags(nameof(IAuthClient));

    private static async ValueTask<Error?> HandleAsync(
        [FromBody] IAuthClient.UpdateProfileRequest request,
        UserManager<User> userManager,
        ClaimsPrincipal principal,
        CancellationToken ct)
    {
        var user = (await userManager.GetUserAsync(principal))!;

        user.UserName = request.UserName ?? user.UserName;
        user.AvatarUrl = request.AvatarUrl ?? user.AvatarUrl;

        await userManager.UpdateAsync(user);

        return null;
    }
}
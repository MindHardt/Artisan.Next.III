using System.Security.Claims;
using Client.Features.Auth;
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Data;

namespace Server.Features.Files;

[Handler]
[MapGet(Contracts.GetFileUsage.FullPath)]
public partial class GetFileUsage
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.RequireAuthorization().WithTags(nameof(FileEndpoints));

    private static async ValueTask<Ok<Contracts.GetFileUsage.Response>> HandleAsync(
        EmptyRequest _,
        ClaimsPrincipal principal,
        DataContext dataContext,
        IOptions<FileStorageOptions> fsOptions,
        CancellationToken ct)
    {
        var userId = principal.GetUserId()!.Value;

        var queryResult = await dataContext.Users
            .Where(x => x.Id == userId)
            .Select(x => new
            {
                x.CustomStorageLimit,
                
                TotalFileSize = x.Files
                    .Select(file => new { file.Hash, file.Size })
                    .Distinct()
                    .Sum(file => file.Size)
            })
            .FirstAsync(ct);

        return TypedResults.Ok(new Contracts.GetFileUsage.Response(
            FileSize.From(queryResult.TotalFileSize),
            FileSize.From(queryResult.CustomStorageLimit ?? fsOptions.Value.DefaultLimit)));
    }
}
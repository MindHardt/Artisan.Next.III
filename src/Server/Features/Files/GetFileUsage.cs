using System.Security.Claims;
using Client.Features.Auth;
using Client.Features.Files;
using Client.Infrastructure;
using ErrorOr;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Data;
using Server.Features.Auth;
using Server.Infrastructure;
using FileSize = Client.Features.Files.FileSize;

namespace Server.Features.Files;

[Handler]
[MapGet(IFileClient.GetFileUsagePath)]
public partial class GetFileUsage
{
    internal static Results<Ok<IFileClient.GetFileUsageResponse>, ProblemHttpResult> TransformResult(
        ErrorOr<IFileClient.GetFileUsageResponse> value) => value.GetHttpResult();
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.RequireAuthorization().WithTags(nameof(IFileClient));

    private static async ValueTask<ErrorOr<IFileClient.GetFileUsageResponse>> HandleAsync(
        [AsParameters] EmptyRequest _,
        ClaimsPrincipal principal,
        DataContext dataContext,
        IOptions<UserOptions> userOptions,
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

        return new IFileClient.GetFileUsageResponse(
            FileSize.From(queryResult.TotalFileSize),
            FileSize.From(queryResult.CustomStorageLimit ?? userOptions.Value.FileStorageLimit));
    }
}
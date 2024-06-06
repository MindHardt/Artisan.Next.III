using System.Diagnostics;
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Server.Features.Files;

[Handler]
[MapGet(Contracts.CloneFile.FullPath)]
public partial class CloneFile
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.WithTags(nameof(FileEndpoints)).RequireAuthorization();

    private static async ValueTask<Results<Ok<FileModel>, ProblemHttpResult, BadRequest, ForbidHttpResult>> HandleAsync(
        Contracts.CloneFile.Request request,
        IHttpClientFactory httpClientFactory,
        IServiceProvider sp,
        CancellationToken ct)
    {
        var httpClient = httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync(request.Url, ct);
        if (response.IsSuccessStatusCode is false)
        {
            return TypedResults.Problem(new ProblemDetails
            {
                Status = (int?)response.StatusCode,
                Detail = "Remote server response does not indicate success."
            });
        }

        var formFile = await response.CreateFormFileAsync(ct);
        
        var innerRequest = new UploadFile.Request(formFile, request.FileScope);
        var innerResult = await sp.GetRequiredService<UploadFile.Handler>()
            .HandleAsync(innerRequest, ct);

        return innerResult.Result switch
        {
            Ok<FileModel> result => result,
            ForbidHttpResult result => result,
            _ => throw new UnreachableException()
        };
    }
}
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Data;
using CacheControlHeaderValue = Microsoft.Net.Http.Headers.CacheControlHeaderValue;

namespace Server.Features.Files;

[Handler]
[MapGet(Contracts.GetFile.FullPath)]
public partial class GetFile
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        (endpoint as RouteHandlerBuilder)?
        .Produces(StatusCodes.Status410Gone)
        .WithTags(nameof(FileEndpoints));

    private static async ValueTask<Results<FileStreamHttpResult, NotFound,ProblemHttpResult>> HandleAsync(
        [AsParameters] Contracts.GetFile.Request request,
        DataContext dataContext,
        IFileStorage fs,
        HttpResponse response,
        ILogger<GetFile> logger,
        CancellationToken ct)
    {
        var file = await dataContext.Files
            .FirstOrDefaultAsync(x => x.Identifier == request.Identifier, ct);
        if (file is null)
        {
            return TypedResults.NotFound();
        }

        response.Headers.CacheControl = CacheControlHeaderValue.PublicString;
        var fileName = request.Name switch
        {
            Contracts.GetFile.Name.Original => file.OriginalName,
            _ => file.Identifier.Value
        };

        try
        {
            var fileStream = await fs.GetFileStream(file.Hash, ct);
            return TypedResults.File(
                fileStream,
                file.ContentType,
                fileName,
                file.CreatedAt);
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "There was an exception when retrieving file content. Serving 500 response");
            return TypedResults.Problem(new ProblemDetails
            {
                Detail = e.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }
}
using Client.Features.Files;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using CacheControlHeaderValue = Microsoft.Net.Http.Headers.CacheControlHeaderValue;

namespace Server.Features.Files;

[Handler]
[MapGet(IFileClient.GetFilePath)]
public partial class GetFile
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.WithTags(nameof(IFileClient));

    private static async ValueTask<Results<FileStreamHttpResult, NotFound, StatusCodeHttpResult>> HandleAsync(
        [AsParameters] IFileClient.GetFileRequest request,
        DataContext dataContext,
        IFileContentStorage fs,
        HttpResponse response,
        CancellationToken ct)
    {
        var file = await dataContext.Files
            .FirstOrDefaultAsync(x => x.Identifier == request.FileName, ct);
        if (file is null)
        {
            return TypedResults.NotFound();
        }

        response.Headers.CacheControl = CacheControlHeaderValue.PublicString;
        var fileName = request.OriginalName
            ? file.OriginalName
            : file.Identifier.Value;

        var fileStream = await fs.GetFileStream(file.Hash, ct);
        return TypedResults.File(
            fileStream,
            file.ContentType,
            fileName,
            file.CreatedAt);
    }
}
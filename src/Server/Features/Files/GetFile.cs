using System.Net.Cache;
using System.Net.Http.Headers;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Data;
using CacheControlHeaderValue = Microsoft.Net.Http.Headers.CacheControlHeaderValue;

namespace Server.Features.Files;

[Handler]
[MapGet(Contracts.GetFile.FullPath)]
public partial class GetFile
{
    private static async ValueTask<Results<FileStreamHttpResult, NotFound, NoContent>> HandleAsync(
        Contracts.GetFile.Request request,
        DataContext dataContext,
        IOptions<FileStorageOptions> fsOptions,
        HttpResponse response,
        CancellationToken ct)
    {
        var file = await dataContext.Files
            .FirstOrDefaultAsync(x => x.Identifier == request.Identifier, ct);
        if (file is null)
        {
            return TypedResults.NotFound();
        }
        
        response.Headers.CacheControl = CacheControlHeaderValue.PublicString;

        var path = Path.Combine(fsOptions.Value.Directory, file.Hash.Value);
        if (File.Exists(path) is false)
        {
            return TypedResults.NoContent();
        }
        
        var contentStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        return TypedResults.File(
            contentStream, 
            file.ContentType, 
            file.Identifier.Value, 
            file.CreatedAt);
    }
}
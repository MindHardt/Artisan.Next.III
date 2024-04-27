using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Data;

namespace Server.Features.Files;

[Handler]
[MapGet(Contracts.GetFile.FullPath)]
public partial class GetFile
{
    private static async ValueTask<Results<FileStreamHttpResult, NotFound>> HandleAsync(
        Contracts.GetFile.Request request,
        DataContext dataContext,
        IOptions<FileStorageOptions> fsOptions,
        CancellationToken ct)
    {
        var file = await dataContext.Files
            .FirstOrDefaultAsync(x => x.Identifier == request.Identifier, ct);
        if (file is null)
        {
            return TypedResults.NotFound();
        }

        var path = Path.Combine(fsOptions.Value.Directory, file.Hash.Value);
        var contentStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

        return TypedResults.File(contentStream, file.ContentType, file.Identifier.Value);
    }
}
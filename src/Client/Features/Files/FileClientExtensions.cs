using Contracts;
using ErrorOr;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Features.Files;

internal static class FileClientExtensions
{
    public static async Task<ErrorOr<FileModel>> UploadFile(
        this IFileClient client,
        UploadFile.Request<IBrowserFile> request,
        CancellationToken ct = default)
    {
        await using var contentStream = request.File.OpenReadStream(long.MaxValue, ct);
        return await client.UploadFile(new UploadFile.Request<UploadFile.PostedFile>(
            new UploadFile.PostedFile(contentStream, request.File.ContentType, request.File.Name),
            request.Scope),
            ct);
    }
}
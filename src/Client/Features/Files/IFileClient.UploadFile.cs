using ErrorOr;
using Microsoft.AspNetCore.Components.Forms;
using Refit;

namespace Client.Features.Files;

public partial interface IFileClient
{
    public const string UploadFilePath = $"{Prefix}/upload";
    [Post(UploadFilePath), Multipart]
    protected Task<ErrorOr<FileModel>> UploadFile(
        StreamPart file,
        string scope);

    public async Task<ErrorOr<FileModel>> UploadFile(UploadFileRequest request, CancellationToken ct = default)
    {
        await using var contentStream = request.File.OpenReadStream(long.MaxValue, ct);
        return await UploadFile(
            new StreamPart(contentStream, request.File.Name, request.File.ContentType, nameof(request.File)),
            request.Scope.ToString());
    }
    public record UploadFileRequest(
        IBrowserFile File,
        FileScope Scope);
}
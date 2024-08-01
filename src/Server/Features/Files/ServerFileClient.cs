using Client.Features.Files;
using Client.Infrastructure;
using ErrorOr;
using Refit;

namespace Server.Features.Files;

[RegisterScoped]
public class ServerFileClient(IServiceProvider sp) : IFileClient
{
    public async Task<ErrorOr<IFileClient.SearchFilesResponse>> SearchFiles(IFileClient.SearchFilesRequest request, CancellationToken ct = default)
        => await sp.GetRequiredService<SearchFiles.Handler>().HandleAsync(request, ct);

    public async Task<ErrorOr<IFileClient.GetFileUsageResponse>> GetFileUsage(CancellationToken ct = default)
        => await sp.GetRequiredService<GetFileUsage.Handler>().HandleAsync(new EmptyRequest(), ct);

    public Task<ErrorOr<FileModel>> UploadFile(StreamPart file, string scope)
        => throw new NotSupportedException();
    
    public Task<ErrorOr<FileModel>> DeleteFile(IFileClient.DeleteFileRequest request, CancellationToken ct = default)
        => throw new NotSupportedException();
}
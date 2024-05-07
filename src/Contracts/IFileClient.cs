using ErrorOr;

namespace Contracts;

public interface IFileClient
{
    public Task<ErrorOr<FileModel>> UploadFile(UploadFile.Request<UploadFile.PostedFile> request, CancellationToken ct = default);
    public Task<ErrorOr<SearchFiles.Response>> SearchFiles(SearchFiles.Request request, CancellationToken ct = default);
    public Task<ErrorOr<FileModel>> DeleteFile(DeleteFile.Request request, CancellationToken ct = default);
    public Task<ErrorOr<GetFileUsage.Response>> GetUsage(CancellationToken ct = default);
}
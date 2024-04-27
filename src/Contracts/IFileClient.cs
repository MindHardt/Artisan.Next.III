namespace Contracts;

public interface IFileClient
{
    public Task<FileModel> UploadFile(UploadFile.Request<UploadFile.PostedFile> request, CancellationToken ct = default);
    public Task<FileModel[]> SearchFiles(SearchFiles.Request request, CancellationToken ct = default);
    public Task<FileModel> DeleteFile(DeleteFile.Request request, CancellationToken ct = default);
}
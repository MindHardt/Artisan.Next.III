using ErrorOr;
using Refit;

namespace Client.Features.Files;

public partial interface IFileClient
{
    public const string DeleteFilePath = $"{Prefix}/delete";
    [Delete(DeleteFilePath)]
    public Task<ErrorOr<FileModel>> DeleteFile(
        [Query] DeleteFileRequest request, CancellationToken ct = default);
    public record DeleteFileRequest(
        FileIdentifier FileName);
}
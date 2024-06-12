using Contracts;

namespace Server.Features.Files;

public interface IFileStorage
{
    public Task<Stream> GetFileStream(FileHashString hash, CancellationToken ct = default);
    public Task SaveFile(Stream content, FileHashString hash, CancellationToken ct = default);
    public Task<bool> FileExists(FileHashString hash, CancellationToken ct = default);
    public Task DeleteFile(FileHashString hash, CancellationToken ct = default);
}
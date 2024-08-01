using Microsoft.Extensions.Options;
using FileHashString = Client.Features.Files.FileHashString;

namespace Server.Features.Files;

public class LocalFileContentStorage : IFileContentStorage
{
    private readonly DirectoryInfo _directory;

    public LocalFileContentStorage(IOptions<LocalFileStorageOptions> options)
    {
        _directory = new DirectoryInfo(options.Value.Directory);
        _directory.Create();
    }

    public Task<Stream> GetFileStream(FileHashString hash, CancellationToken ct = default) => Task.FromResult((Stream)
        new FileStream(GetPath(hash), FileMode.Open, FileAccess.Read, FileShare.Read));

    public async Task SaveFile(Stream content, FileHashString hash, CancellationToken ct = default)
    {
        await using var fs = new FileStream(GetPath(hash), FileMode.Create, FileAccess.Write, FileShare.None);
        await content.CopyToAsync(fs, ct);
    }

    public Task<bool> FileExists(FileHashString hash, CancellationToken ct = default)
        => Task.FromResult(File.Exists(GetPath(hash)));

    public Task DeleteFile(FileHashString hash, CancellationToken ct = default)
    {
        File.Delete(GetPath(hash));
        return Task.CompletedTask;
    }

    private string GetPath(FileHashString hash) => Path.Combine(_directory.FullName, hash.Value);
}

public record LocalFileStorageOptions
{
    public const string Section = "FileStorage";

    public string Directory { get; set; } = "FileStorage";
}
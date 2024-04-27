using Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Server.Features.Files;

[RegisterScoped]
public class FileClient(IServiceProvider sp) : IFileClient
{
    public async Task<FileModel> UploadFile(Contracts.UploadFile.Request<Contracts.UploadFile.PostedFile> request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<UploadFile.Handler>();
        var formFile = new FormFile(
            request.File.Content, 0,
            request.File.Content.Length,
            nameof(request.File),
            request.File.FileName);
        
        var result = await handler.HandleAsync(new UploadFile.Request(formFile, request.Scope), ct);
        return result.Value!;
    }

    public async Task<FileModel[]> SearchFiles(Contracts.SearchFiles.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<Files.SearchFiles.Handler>();
        var result = await handler.HandleAsync(request, ct);

        return result.Value!;
    }

    public async Task<FileModel> DeleteFile(Contracts.DeleteFile.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<DeleteFile.Handler>();
        var result = await handler.HandleAsync(new Contracts.DeleteFile.Request(request.Identifier), ct);

        return ((Ok<FileModel>)result.Result).Value!;
    }
}
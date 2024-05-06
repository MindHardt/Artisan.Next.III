using Contracts;
using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Server.Features.Shared;

namespace Server.Features.Files;

[RegisterScoped]
public class FileClient(IServiceProvider sp) : IFileClient
{
    public async Task<ErrorOr<FileModel>> UploadFile(Contracts.UploadFile.Request<Contracts.UploadFile.PostedFile> request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<UploadFile.Handler>();
        var formFile = new FormFile(
            request.File.Content, 0,
            request.File.Content.Length,
            nameof(request.File),
            request.File.FileName);
        
        return (await handler.HandleAsync(new UploadFile.Request(formFile, request.Scope), ct)).AsErrorOr<FileModel>();
    }

    public async Task<ErrorOr<Contracts.SearchFiles.Response>> SearchFiles(Contracts.SearchFiles.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<SearchFiles.Handler>();
        return (await handler.HandleAsync(request, ct)).AsErrorOr<Contracts.SearchFiles.Response>();
    }

    public async Task<ErrorOr<FileModel>> DeleteFile(Contracts.DeleteFile.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<DeleteFile.Handler>();
        return (await handler.HandleAsync(request, ct)).AsErrorOr<FileModel>();
    }
}
using Contracts;
using ErrorOr;
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
        var innerRequest = new SearchFiles.Request(
            request.Regex, request.RestrictedToScope, request.Page, request.PageSize);
        return (await handler.HandleAsync(innerRequest, ct)).AsErrorOr<Contracts.SearchFiles.Response>();
    }

    public async Task<ErrorOr<FileModel>> DeleteFile(Contracts.DeleteFile.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<DeleteFile.Handler>();
        return (await handler.HandleAsync(request, ct)).AsErrorOr<FileModel>();
    }

    public async Task<ErrorOr<Contracts.GetFileUsage.Response>> GetFileUsage(CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<GetFileUsage.Handler>();
        return (await handler.HandleAsync(new EmptyRequest(), ct)).AsErrorOr<Contracts.GetFileUsage.Response>();
    }

    public async Task<ErrorOr<IReadOnlyCollection<FileIdentifier>>> GetBlogposts(CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<GetBlogposts.Handler>();
        return (await handler.HandleAsync(new EmptyRequest(), ct)).AsErrorOr<IReadOnlyCollection<FileIdentifier>>();
    }
}
using ErrorOr;
using Refit;

namespace Client.Features.Files;

public partial interface IFileClient
{
    public const string SearchFilesPath = $"{Prefix}/search";
    [Get(SearchFilesPath)]
    public Task<ErrorOr<SearchFilesResponse>> SearchFiles(
        [Query] SearchFilesRequest request,
        CancellationToken ct = default);
        
    public record SearchFilesRequest(
        string? Regex = null,
        FileScope? RestrictedToScope = null,
        int Page = 0,
        int PageSize = 10);

    public record SearchFilesResponse(
        IReadOnlyCollection<FileModel> Files,
        int TotalFiles);
}
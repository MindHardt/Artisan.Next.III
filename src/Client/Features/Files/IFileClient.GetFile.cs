using System.Web;

namespace Client.Features.Files;

public partial interface IFileClient
{
    public const string GetFilePath = $"{Prefix}/download";
    public string BuildFileUrl(GetFileRequest request)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query[nameof(request.FileName)] = request.FileName.Value;
        query[nameof(request.OriginalName)] = request.OriginalName.ToString();

        return $"{GetFilePath}?{query}";
    }

    public string BuildFileUrl(FileModel file, bool originalName = true) 
        => BuildFileUrl(new GetFileRequest(file.Identifier, originalName));
    
    public record GetFileRequest(
        FileIdentifier FileName,
        bool OriginalName = true);
}
namespace Contracts;

public static class UploadFile
{
    public const string Path = "upload";
    public const string FullPath = $"{FileEndpoints.FullPath}/{Path}";

    public record Request<TFile>(TFile File, FileScope Scope);

    public record PostedFile(Stream Content, string ContentType, string FileName);
}
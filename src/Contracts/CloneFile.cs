namespace Contracts;

public static class CloneFile
{
    public const string Path = "clone";
    public const string FullPath = $"{FileEndpoints.FullPath}/{Path}";
    
    public record Request(string Url, FileScope FileScope);
}
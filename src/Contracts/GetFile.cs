namespace Contracts;

public static class GetFile
{
    public const string Path = $"{{{nameof(Request.Identifier)}}}";
    public const string FullPath = $"{FileEndpoints.FullPath}/{Path}";
    
    public record Request(
        FileIdentifier Identifier);
}
namespace Contracts;

public static class GetFile
{
    public const string Path = $"{{{nameof(Request.Identifier)}}}";
    public const string FullPath = $"{FileEndpoints.FullPath}/{Path}";

    public enum Name
    {
        Server,
        Original
    }

    public record Request(
        FileIdentifier Identifier,
        Name? Name = null);
}
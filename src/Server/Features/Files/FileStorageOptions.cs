namespace Server.Features.Files;

public record FileStorageOptions
{
    public const string Section = "FileStorage";
    
    public string Directory { get; set; } = "FileStorage";
}
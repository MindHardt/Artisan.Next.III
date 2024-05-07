namespace Server.Features.Files;

public record FileStorageOptions
{
    public const string Section = "FileStorage";
    
    public long DefaultLimit { get; set; } = 10_485_760; // 10 MB
    public string Directory { get; set; } = "FileStorage";
}
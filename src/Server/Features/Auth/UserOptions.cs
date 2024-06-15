namespace Server.Features.Auth;

public record UserOptions
{
    public const string Section = "Users";
    
    public long FileStorageLimit { get; set; } = 10_485_760; // 10 MB
}
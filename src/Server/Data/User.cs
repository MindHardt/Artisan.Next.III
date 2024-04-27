using Microsoft.AspNetCore.Identity;

namespace Server.Data;

public class User : IdentityUser<int>
{
    public required string AvatarUrl { get; set; }
}
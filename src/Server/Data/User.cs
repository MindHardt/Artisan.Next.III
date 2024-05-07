using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Data;

public class User : IdentityUser<int>
{
    public required string AvatarUrl { get; set; }
    public long? CustomStorageLimit { get; set; }

    public ICollection<StorageFile> Files { get; set; } = null!;
    
    internal class EntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(x => x.Files)
                .WithOne(x => x.Uploader)
                .HasForeignKey(x => x.UploaderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
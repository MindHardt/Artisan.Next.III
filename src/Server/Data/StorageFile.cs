using System.ComponentModel.DataAnnotations;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Data;

public record StorageFile
{
    public const int MaxFileNameLength = 255;
    
    public required FileIdentifier Identifier { get; set; }
    
    public required FileHashString Hash { get; set; }
    
    [MaxLength(MaxFileNameLength)]
    public required string OriginalName { get; set; }
    
    [MaxLength(255)]
    public required string ContentType { get; set; }
    
    public required long Size { get; set; }
    public required FileScope Scope { get; set; }
    
    public required DateTimeOffset CreatedAt { get; set; }
    
    public User? Uploader { get; set; }
    public required int UploaderId { get; set; }

    public class Configuration : IEntityTypeConfiguration<StorageFile>
    {
        public void Configure(EntityTypeBuilder<StorageFile> builder)
        {
            builder.HasKey(x => x.Identifier);
            builder.Property(x => x.Identifier)
                .HasConversion(
                    x => x.Value,
                    x => FileIdentifier.From(x))
                .HasMaxLength(FileIdentifier.MaxLength);
            
            builder.Property(x => x.Hash)
                .HasConversion(
                    x => x.Value,
                    x => FileHashString.From(x))
                .UseCollation("C")
                .HasMaxLength(FileHashString.ExpectedLength);
            
            builder.HasIndex(x => new { x.Hash, ServerName = x.Identifier })
                .IncludeProperties(x => new { x.Scope, x.OriginalName });
            
            builder.Property(x => x.Scope)
                .HasConversion<string>()
                .HasMaxLength(Enum.GetValues<FileScope>().Max(x => x.ToString().Length));
        }
    }
}
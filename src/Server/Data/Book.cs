using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Data;

public record Book
{
    public required BookUrlName UrlName { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Author { get; set; }
    public required string Text { get; set; }
    public required string? ImageUrl { get; set; }
    public required bool IsPublic { get; set; }
    public required DateTimeOffset LastUpdated { get; set; }
    
    public required int? OwnerId { get; set; }
    public User? Owner { get; set; }
    
    public ICollection<BookInvite>? Invites { get; set; }
    public ICollection<BookVisit>? Visits { get; set; }

    public class EntityConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(x => x.UrlName);
            builder.Property(x => x.UrlName)
                .HasConversion(
                    x => x.Value,
                    x => BookUrlName.From(x))
                .HasMaxLength(BookUrlName.MaxLength);

            builder.HasOne(x => x.Owner)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.OwnerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
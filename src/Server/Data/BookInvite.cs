using System.ComponentModel.DataAnnotations;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Data;

public record BookInvite
{
    public required BookInviteKey Key { get; set; }

    public BookUrlName BookName { get; set; }
    public Book? Book { get; set; }

    public BookInviteStatus Status { get; set; }

    public class EntityConfiguration : IEntityTypeConfiguration<BookInvite>
    {
        public void Configure(EntityTypeBuilder<BookInvite> builder)
        {
            builder.HasKey(x => x.Key);
            builder.Property(x => x.Key)
                .HasMaxLength(BookInviteKey.MaxLength)
                .HasConversion(
                    x => x.Value,
                    x => BookInviteKey.From(x));
            builder.Property(x => x.BookName)
                .HasMaxLength(BookUrlName.MaxLength)
                .HasConversion(
                    x => x.Value,
                    x => BookUrlName.From(x));
            builder.HasOne(x => x.Book)
                .WithMany(x => x.Invites)
                .HasForeignKey(x => x.BookName)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

public enum BookInviteStatus
{
    Active,
    Revoked
}
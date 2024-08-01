using Client.Features.Wiki.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Data;

public record BookVisit
{
    public BookUrlName BookName { get; set; }
    public Book? Book { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public class EntityConfiguration : IEntityTypeConfiguration<BookVisit>
    {
        public void Configure(EntityTypeBuilder<BookVisit> builder)
        {
            builder.HasKey(x => new { BookName = x.BookName, x.UserId });
            builder.Property(x => x.BookName)
                .HasMaxLength(BookUrlName.MaxLength)
                .HasConversion(
                    x => x.Value,
                    x => BookUrlName.From(x));
            builder.HasOne(x => x.Book)
                .WithMany(x => x.Visits)
                .HasForeignKey(x => x.BookName)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
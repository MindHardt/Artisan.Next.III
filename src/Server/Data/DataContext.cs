using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Data.Alids;

namespace Server.Data;

public class DataContext(DbContextOptions<DataContext> options) 
    : IdentityDbContext<User, IdentityRole<int>, int>(options), IDataProtectionKeyContext
{
    public DbSet<StorageFile> Files => Set<StorageFile>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
    public DbSet<BookInvite> BookInvites => Set<BookInvite>();
    public DbSet<BookVisit> BookVisits => Set<BookVisit>(); 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.ConfigureAlidEntityConversions();
        base.ConfigureConventions(configurationBuilder);
    }
}
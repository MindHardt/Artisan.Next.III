using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Server.Data;

public class DataContext(DbContextOptions<DataContext> options) 
    : IdentityDbContext<User, IdentityRole<int>, int>(options), IDataProtectionKeyContext
{
    public DbSet<StorageFile> Files => Set<StorageFile>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
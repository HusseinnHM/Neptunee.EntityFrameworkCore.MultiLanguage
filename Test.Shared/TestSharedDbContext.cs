using Microsoft.EntityFrameworkCore;
using Neptunee.EntityFrameworkCore.MultiLanguage.Extensions;

namespace Test.Shared;

public class TestSharedDbContext : DbContext
{
    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>().Property(c => c.Id).ValueGeneratedNever();
        modelBuilder.ConfigureMultiLanguage(Database);
        base.OnModelCreating(modelBuilder);
    }
}
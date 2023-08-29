using Microsoft.EntityFrameworkCore;
using Neptunee.EntityFrameworkCore.MultiLanguage.Extensions;
using Sample.Entities;

namespace Sample.DbContexts;

public class SampleDbContext: DbContext
{
    public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options) 
    {
        
    }
    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureMultiLanguage(Database);
        base.OnModelCreating(modelBuilder);
    }
}
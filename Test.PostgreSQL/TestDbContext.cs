using Microsoft.EntityFrameworkCore;
using Test.Shared;

namespace Test.PostgreSQL;

public class TestDbContext : TestSharedDbContext
{


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=0000;Database=TestMultiLanguage;");
        base.OnConfiguring(optionsBuilder);
    }
}
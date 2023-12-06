using Microsoft.EntityFrameworkCore;
using Test.Shared;

namespace Test.SqlServer;

public class TestDbContext : TestSharedDbContext
{


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Database=TestMultiLanguage;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=True;Integrated Security=true");
        base.OnConfiguring(optionsBuilder);
    }
}
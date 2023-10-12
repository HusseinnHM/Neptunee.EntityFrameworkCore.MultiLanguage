using Microsoft.EntityFrameworkCore;
using Test.Shared;

namespace Test.SqlServer;

public class TestDbContext : TestSharedDbContext
{


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=127.0.0.1\\sql,1433;Database=TestMultiLanguage;User ID=sa;Password=Passw0rd;TrustServerCertificate=True");
        base.OnConfiguring(optionsBuilder);
    }
}
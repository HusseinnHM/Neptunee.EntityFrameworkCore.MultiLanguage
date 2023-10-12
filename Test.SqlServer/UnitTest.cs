using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Neptunee.EntityFrameworkCore.MultiLanguage;
using Neptunee.EntityFrameworkCore.MultiLanguage.HostedServices;
using Neptunee.EntityFrameworkCore.MultiLanguage.Types;
using Test.Shared;
using Xunit.Abstractions;

namespace Test.SqlServer;

public class UnitTest
{
    private readonly Country _defaultCountry;
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _defaultCountry = new Country
        {
            Id = new Guid("A1484E25-B03E-47AD-B9BF-95FD566940B3"),
            Name = new MultiLanguageProperty("test default en")
        };
        _defaultCountry.Name.Upsert(TestLanguageKeys.Ar, "test ar");
        _defaultCountry.Name.Upsert(TestLanguageKeys.Fr, "test Fr");

        Initial();
    }

    [Fact]
    public async Task Add()
    {
        await using var context = new TestDbContext();
        var country = new Country
        {
            Id = Guid.NewGuid(),
            Name = new MultiLanguageProperty("test add in default")
        };
        country.Name.Upsert(TestLanguageKeys.Ar, "test add ar");
        country.Name.Upsert(TestLanguageKeys.Fr, "test add en");
        context.Countries.Add(country);
        await context.SaveChangesAsync();

        Assert.True(context.Countries.Count() > 1);
    }

    [Theory]
    [InlineData("")]
    [InlineData(nameof(TestLanguageKeys.Ar))]
    [InlineData(nameof(TestLanguageKeys.Fr))]
    [InlineData(nameof(TestLanguageKeys.UnKnown))]
    public async Task Get(string languageKeyStr)
    {
        await using var context = new TestDbContext();
        var languageKey = LanguageKey.Pars(languageKeyStr);
        var query = context.Countries.AsNoTracking().Where(c => c.Id == _defaultCountry.Id).Select(c => new
        {
            c.Id,
            GetIn = c.Name.GetIn(languageKey),
            GetOrDefaultIn = c.Name.GetOrDefaultIn(languageKey),
            ContainsIn = c.Name.ContainsIn(languageKey)
        });

        _testOutputHelper.WriteLine( Environment.NewLine + Environment.NewLine + query.ToQueryString());
    }


    [Theory]
    [InlineData("")]
    [InlineData(nameof(TestLanguageKeys.Ar))]
    [InlineData(nameof(TestLanguageKeys.Fr))]
    public async Task Modify(string languageKeyStr)
    {
        var languageKey = LanguageKey.Pars(languageKeyStr);
        await using var context = new TestDbContext();
        var country = await context.Countries.FindAsync(_defaultCountry.Id);
        var newValue = $"test {languageKey} modified at {DateTime.Now}";

        country!.Name.Upsert(languageKey, newValue);
        context.Update(country);
        await context.SaveChangesAsync();

        var lastValue = context.Countries
            .Where(c => c.Id == country.Id)
            .Select(c => c.Name.GetOrDefaultIn(languageKey))
            .First();
        Assert.Equal(newValue, lastValue);
    }

    private void Initial()
    {
        using var context = new TestDbContext();
        context.Database.Migrate();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<TestDbContext>();
        var createDbFunctions = new CreateMultiLanguageDbFunctions<TestDbContext>(serviceCollection.BuildServiceProvider());
        createDbFunctions.StartAsync(default).GetAwaiter().GetResult();
        if (!context.Countries.Any())
        {
            context.Add(_defaultCountry);
            context.SaveChanges();
        }
    }
}
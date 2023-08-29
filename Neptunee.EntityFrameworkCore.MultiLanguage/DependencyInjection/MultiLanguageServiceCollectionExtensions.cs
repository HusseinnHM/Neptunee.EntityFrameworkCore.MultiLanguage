using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Neptunee.EntityFrameworkCore.MultiLanguage.HostedServices;

namespace Neptunee.EntityFrameworkCore.MultiLanguage.DependencyInjection;

public static class MultiLanguageServiceCollectionExtensions
{
    public static IServiceCollection AddMultiLanguage<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        => services.AddHostedService<CreateMultiLanguageDbFunctions<TDbContext>>();
}
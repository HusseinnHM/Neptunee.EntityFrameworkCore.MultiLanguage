using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neptunee.EntityFrameworkCore.MultiLanguage.Exceptions;
using Neptunee.EntityFrameworkCore.MultiLanguage.Extensions;
using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Neptunee.EntityFrameworkCore.MultiLanguage.HostedServices;

public class CreateMultiLanguageDbFunctions<TDbContext> : IHostedService where TDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;

    public CreateMultiLanguageDbFunctions(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();
        await context.Database.ExecuteSqlRawAsync(CreateGetOrDefaultInCommand(context.Database.ProviderName!), cancellationToken: cancellationToken);
        await context.Database.ExecuteSqlRawAsync(CreateGetInCommand(context.Database.ProviderName!), cancellationToken: cancellationToken);
        await context.Database.ExecuteSqlRawAsync(CreateGetDefaultCommand(context.Database.ProviderName!), cancellationToken: cancellationToken);
        await context.Database.ExecuteSqlRawAsync(CreateEfContainsValueInCommand(context.Database.ProviderName!),
            cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }


    private string CreateGetOrDefaultInCommand(string providerName)
    {
        return string.Equals(providerName, ProviderNames.PostgreSql,
            StringComparison.OrdinalIgnoreCase)
            ? $@"
                    CREATE OR REPLACE FUNCTION {Helper.FunctionName(nameof(MultiLanguageFunctions.GetOrDefaultIn))}(PROP JSONB, LANGUAGEKEY TEXT)
                     RETURNS TEXT
                     LANGUAGE PLPGSQL
                    AS $FUNCTION$
                    BEGIN
	                    IF (PROP ? LANGUAGEKEY) THEN
		                    RETURN PROP ->> LANGUAGEKEY;
	                    ELSE 
		                    RETURN PROP ->> '';
	                    END IF;
                    END;
                    $FUNCTION$
                    ;"
            : throw new ProviderNotSupportedException(providerName);
    }

    private string CreateGetInCommand(string providerName)
    {
        return string.Equals(providerName, ProviderNames.PostgreSql,
            StringComparison.OrdinalIgnoreCase)
            ? $@"
                    CREATE OR REPLACE FUNCTION {Helper.FunctionName(nameof(MultiLanguageProperty.GetIn))}(PROP JSONB, LANGUAGEKEY TEXT)
                     RETURNS TEXT
                     LANGUAGE PLPGSQL
                    AS $FUNCTION$
                    BEGIN
                        RETURN PROP ->> LANGUAGEKEY;
                    END;
                    $FUNCTION$
                    ;"
            : throw new ProviderNotSupportedException(providerName);
    }

    private string CreateGetDefaultCommand(string providerName)
    {
        return string.Equals(providerName, ProviderNames.PostgreSql,
            StringComparison.OrdinalIgnoreCase)
            ? $@"
                    CREATE OR REPLACE FUNCTION {Helper.FunctionName(nameof(MultiLanguageFunctions.GetDefault))}(PROP JSONB)
                     RETURNS TEXT
                     LANGUAGE PLPGSQL
                    AS $FUNCTION$
                    BEGIN
                        RETURN PROP ->> '';
                    END;
                    $FUNCTION$
                    ;"
            : throw new ProviderNotSupportedException(providerName);
    }

    private string CreateEfContainsValueInCommand(string providerName)
    {
        return string.Equals(providerName, ProviderNames.PostgreSql,
            StringComparison.OrdinalIgnoreCase)
            ? $@"
                    CREATE OR REPLACE FUNCTION {Helper.FunctionName(nameof(MultiLanguageFunctions.ContainsIn))}(PROP JSONB, LANGUAGEKEY TEXT)
                     RETURNS BOOL
                     LANGUAGE PLPGSQL
                    AS $FUNCTION$
                    BEGIN
                    	RETURN PROP ? LANGUAGEKEY;
                    END;
                    $FUNCTION$
                    ;"
            : throw new ProviderNotSupportedException(providerName);
    }
}
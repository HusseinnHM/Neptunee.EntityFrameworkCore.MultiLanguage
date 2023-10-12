using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        var database = serviceScope.ServiceProvider.GetRequiredService<TDbContext>().Database;

        if (database.IsPostgreSqlProvider())
        {
            foreach (var script in PostgreSqlFunctionsScripts)
            {
                await database.ExecuteSqlRawAsync(script, cancellationToken);
            }
        }

        else if (database.IsSqlServerProvider())
        {
            foreach (var script in DeleteSqlServerFunctionsIfExistsScripts())
            {
                await database.ExecuteSqlRawAsync(script, cancellationToken: cancellationToken);
            }

            foreach (var script in SqlServerFunctionsScripts)
            {
                await database.ExecuteSqlRawAsync(script, cancellationToken: cancellationToken);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }


    private IEnumerable<string> DeleteSqlServerFunctionsIfExistsScripts()
    {
        foreach (var functionName in new[]
                 {
            Helper.FunctionName(nameof(MultiLanguageFunctions.GetOrDefaultIn)),
            Helper.FunctionName(nameof(MultiLanguageFunctions.GetIn)),
            Helper.FunctionName(nameof(MultiLanguageFunctions.GetDefault)),
            Helper.FunctionName(nameof(MultiLanguageFunctions.ContainsIn)),
                 })
        {
            yield return $"""
               IF OBJECT_ID (N'{functionName}', N'FN') IS NOT NULL
               DROP FUNCTION {functionName};
             """;
        }
    }

    private static readonly string[] PostgreSqlFunctionsScripts =
    {
        $"""
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
            ;
         """,
        $"""
         
            CREATE OR REPLACE FUNCTION {Helper.FunctionName(nameof(MultiLanguageProperty.GetIn))}(PROP JSONB, LANGUAGEKEY TEXT)
             RETURNS TEXT
             LANGUAGE PLPGSQL
            AS $FUNCTION$
            BEGIN
                RETURN PROP ->> LANGUAGEKEY;
            END;
            $FUNCTION$
            ;
         """,
        $"""
         
            CREATE OR REPLACE FUNCTION {Helper.FunctionName(nameof(MultiLanguageFunctions.GetDefault))}(PROP JSONB)
             RETURNS TEXT
             LANGUAGE PLPGSQL
            AS $FUNCTION$
            BEGIN
                RETURN PROP ->> '';
            END;
            $FUNCTION$
            ;
         """,
        $"""
         
            CREATE OR REPLACE FUNCTION {Helper.FunctionName(nameof(MultiLanguageFunctions.ContainsIn))}(PROP JSONB, LANGUAGEKEY TEXT)
             RETURNS BOOL
             LANGUAGE PLPGSQL
            AS $FUNCTION$
            BEGIN
            	RETURN PROP ? LANGUAGEKEY;
            END;
            $FUNCTION$
            ;
         """
    };

    private static readonly string[] SqlServerFunctionsScripts =
    {
        $"""
            CREATE FUNCTION {Helper.FunctionName(nameof(MultiLanguageFunctions.GetOrDefaultIn))}(@PROP NVARCHAR(MAX),@LANGUAGEKEY NVARCHAR(10))
             RETURNS NVARCHAR(MAX)
            AS
            BEGIN
              IF (@LANGUAGEKEY != '' AND JSON_PATH_EXISTS(@PROP,'$.'+@LANGUAGEKEY) > 0)
                   RETURN JSON_VALUE(@PROP,'$.'+@LANGUAGEKEY);
               ELSE
                   RETURN JSON_VALUE(@PROP,'$.""');
               RETURN '';
            END;
            ;
         """,
        $"""
            CREATE FUNCTION {Helper.FunctionName(nameof(MultiLanguageProperty.GetIn))}(@PROP NVARCHAR(MAX),@LANGUAGEKEY NVARCHAR(10))
             RETURNS NVARCHAR(MAX)
            AS
            BEGIN
                RETURN JSON_VALUE(@PROP,'$.'+@LANGUAGEKEY);
            END;
            ;
         """,
        $"""
            CREATE FUNCTION {Helper.FunctionName(nameof(MultiLanguageFunctions.GetDefault))}(@PROP NVARCHAR(MAX))
             RETURNS NVARCHAR(MAX)
            AS
            BEGIN
                RETURN JSON_VALUE(@PROP,'$.""');
            END;
            ;
         """,
        $"""
            CREATE FUNCTION {Helper.FunctionName(nameof(MultiLanguageFunctions.ContainsIn))}(@PROP NVARCHAR(MAX), @LANGUAGEKEY NVARCHAR(10))
             RETURNS BIT
            AS
            BEGIN
            	RETURN JSON_PATH_EXISTS(@PROP,'$.'+@LANGUAGEKEY);
            END;
            ;
         """
    };
}
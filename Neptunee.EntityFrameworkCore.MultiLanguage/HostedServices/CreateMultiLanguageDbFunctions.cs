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
            Helper.FunctionName(nameof(MultiLanguageFunctions.GetOrFirstIn)),
            Helper.FunctionName(nameof(MultiLanguageFunctions.GetIn)),
            Helper.FunctionName(nameof(MultiLanguageFunctions.GetFirst)),
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
            CREATE OR REPLACE FUNCTION {Helper.FunctionName(nameof(MultiLanguageFunctions.GetOrFirstIn))}(PROP JSONB, LANGUAGEKEY TEXT)
             RETURNS TEXT
             LANGUAGE PLPGSQL
            AS $FUNCTION$
            BEGIN
                LANGUAGEKEY = LOWER(LANGUAGEKEY);
                IF (PROP ? LANGUAGEKEY) THEN
                    RETURN PROP ->> LANGUAGEKEY;
                ELSE
                    RETURN value FROM jsonb_each_text(PROP) LIMIT 1;
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
                RETURN PROP ->> LOWER(LANGUAGEKEY);
            END;
            $FUNCTION$
            ;
         """,
        $"""
         
            CREATE OR REPLACE FUNCTION {Helper.FunctionName(nameof(MultiLanguageFunctions.GetFirst))}(PROP JSONB)
             RETURNS TEXT
             LANGUAGE PLPGSQL
            AS $FUNCTION$
            BEGIN
                RETURN value FROM jsonb_each_text(PROP) LIMIT 1;
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
            	RETURN PROP ? LOWER(LANGUAGEKEY);
            END;
            $FUNCTION$
            ;
         """
    };

    private static readonly string[] SqlServerFunctionsScripts =
    {
        $"""
            CREATE FUNCTION {Helper.FunctionName(nameof(MultiLanguageFunctions.GetOrFirstIn))}(@PROP NVARCHAR(MAX),@LANGUAGEKEY NVARCHAR(10))
             RETURNS NVARCHAR(MAX)
            AS
            BEGIN
              DECLARE @Result AS NVARCHAR(MAX);
              SET @Result = (SELECT value FROM OPENJSON(@PROP) WHERE [key] = LOWER(@LANGUAGEKEY));
                   IF (@Result is null)
              SET @Result = (SELECT TOP 1 value FROM OPENJSON(@PROP));
              RETURN @Result;
            END;
            ;
         """,
        $"""
            CREATE FUNCTION {Helper.FunctionName(nameof(MultiLanguageProperty.GetIn))}(@PROP NVARCHAR(MAX),@LANGUAGEKEY NVARCHAR(10))
             RETURNS NVARCHAR(MAX)
            AS
            BEGIN
                DECLARE @Result AS NVARCHAR(MAX);
                SET @Result = (SELECT value FROM OPENJSON(@PROP) WHERE [key] = LOWER(@LANGUAGEKEY));
                RETURN @Result;
            END;
            ;
         """,
        $"""
            CREATE FUNCTION {Helper.FunctionName(nameof(MultiLanguageFunctions.GetFirst))}(@PROP NVARCHAR(MAX))
             RETURNS NVARCHAR(MAX)
            AS
            BEGIN
                DECLARE @Result AS NVARCHAR(MAX);
                SET @Result = (SELECT TOP 1 value FROM OPENJSON(@PROP));
                RETURN @Result;
            END;
            ;
         """,
        $"""
            CREATE FUNCTION {Helper.FunctionName(nameof(MultiLanguageFunctions.ContainsIn))}(@PROP NVARCHAR(MAX), @LANGUAGEKEY NVARCHAR(10))
             RETURNS BIT
            AS
            BEGIN
            	DECLARE @Result AS BIT;
                  IF (EXISTS(SELECT *
                      FROM OPENJSON(@PROP)
                      WHERE [key] = @LANGUAGEKEY))
                      SET @Result = 1;
                  ELSE
                      SET @Result = 0;
                  RETURN @Result;
            END;
            ;
         """
    };
}
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Neptunee.EntityFrameworkCore.MultiLanguage.Extensions;

internal static class Helper
{
    internal static string FunctionName(string nameOfMethod) => ("MultiLan" + nameOfMethod).ToLower();
    
    internal static bool IsPostgreSqlProvider(this DatabaseFacade database)
    {
        return database.ProviderName?.Equals("Npgsql.EntityFrameworkCore.PostgreSQL", StringComparison.OrdinalIgnoreCase) ?? false;
    }
    internal static bool IsSqlServerProvider(this DatabaseFacade database)
    {
        return database.ProviderName?.Equals("Microsoft.EntityFrameworkCore.SqlServer", StringComparison.OrdinalIgnoreCase) ?? false;
    }

}
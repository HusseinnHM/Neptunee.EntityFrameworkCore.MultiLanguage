using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Neptunee.EntityFrameworkCore.MultiLanguage.Comparers;
using Neptunee.EntityFrameworkCore.MultiLanguage.Converters;
using Neptunee.EntityFrameworkCore.MultiLanguage.Exceptions;
using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Neptunee.EntityFrameworkCore.MultiLanguage.Extensions;

public static class ModelBuilderExtensions
{
    public static void ConfigureMultiLanguage(this ModelBuilder builder, DatabaseFacade database)
    {
        if (database.IsPostgreSqlProvider())
        {
            ConfigureMultiLanguagePostgreSql(builder);
        }
        else if (database.IsSqlServerProvider())
        {
            ConfigureMultiLanguageSqlServer(builder);
        }
        else
        {
            throw new ProviderNotSupportedException(database.ProviderName!);
        }
    }


    #region PostgreSql

    private static void ConfigureMultiLanguagePostgreSql(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var propertyInfo in entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(MultiLanguageProperty)).ToList())
            {
                builder
                    .Entity(entityType.ClrType)
                    .Property(propertyInfo.Name)
                    .HasColumnType("jsonb");
                builder.Entity(entityType.ClrType).Property(propertyInfo.Name).Metadata.SetValueComparer(new MultiLanguagePropertyComparer());
            }
        }

        ConfigureGetInFuncPostgreSql(builder);
        ConfigureGetDefaultFuncPostgreSql(builder);
        ConfigureGetOrDefaultInFuncPostgreSql(builder);
        ConfigureContainsInFuncPostgreSql(builder);
    }

    private static void ConfigureGetInFuncPostgreSql(ModelBuilder builder)
    {
        var getByDbFunc = builder
            .HasDbFunction(typeof(MultiLanguageFunctions).GetMethod(nameof(MultiLanguageFunctions.GetIn))!)
            .HasName(Helper.FunctionName(nameof(MultiLanguageFunctions.GetIn)))
            .IsBuiltIn(false);
        getByDbFunc.HasParameter("prop").HasStoreType("jsonb");
        getByDbFunc.HasParameter("languageKey").HasStoreType("text");
    }

    private static void ConfigureGetDefaultFuncPostgreSql(ModelBuilder builder)
    {
        var getByDbFunc = builder
            .HasDbFunction(typeof(MultiLanguageFunctions).GetMethod(nameof(MultiLanguageFunctions.GetFirst))!)
            .HasName(Helper.FunctionName(nameof(MultiLanguageFunctions.GetFirst)))
            .IsBuiltIn(false);
        getByDbFunc.HasParameter("prop").HasStoreType("jsonb");
    }

    private static void ConfigureGetOrDefaultInFuncPostgreSql(ModelBuilder builder)
    {
        var getByDbFunc = builder
            .HasDbFunction(typeof(MultiLanguageFunctions).GetMethod(nameof(MultiLanguageFunctions.GetOrFirstIn))!)
            .HasName(Helper.FunctionName(nameof(MultiLanguageFunctions.GetOrFirstIn)))
            .IsBuiltIn(false);
        getByDbFunc.HasParameter("prop").HasStoreType("jsonb");
        getByDbFunc.HasParameter("languageKey").HasStoreType("text");
    }

    private static void ConfigureContainsInFuncPostgreSql(ModelBuilder builder)
    {
        var isExistsDbFunc = builder
            .HasDbFunction(typeof(MultiLanguageFunctions).GetMethod(nameof(MultiLanguageFunctions.ContainsIn))!)
            .HasName(Helper.FunctionName(nameof(MultiLanguageFunctions.ContainsIn)))
            .IsBuiltIn(false);
        isExistsDbFunc.HasParameter("prop").HasStoreType("jsonb");
        isExistsDbFunc.HasParameter("languageKey").HasStoreType("text");
    }

    #endregion

    #region SqlServer

    private static void ConfigureMultiLanguageSqlServer(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes().ToList())
        {
            foreach (var propertyInfo in entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(MultiLanguageProperty)).ToList())
            {
                builder
                    .Entity(entityType.ClrType)
                    .Property(propertyInfo.Name).HasConversion<MultiLanguagePropertyValueConverter>(new MultiLanguagePropertyComparer());
            }
        }

        ConfigureGetInFuncSqlServer(builder);
        ConfigureGetDefaultFuncSqlServer(builder);
        ConfigureGetOrDefaultInFuncSqlServer(builder);
        ConfigureContainsInFuncSqlServer(builder);
    }

    private static void ConfigureGetInFuncSqlServer(ModelBuilder builder)
    {
        var getByDbFunc = builder
            .HasDbFunction(typeof(MultiLanguageFunctions).GetMethod(nameof(MultiLanguageFunctions.GetIn))!)
            .HasName(Helper.FunctionName(nameof(MultiLanguageFunctions.GetIn)))
            .IsBuiltIn(false);
        getByDbFunc.HasParameter("prop").HasStoreType("nvarchar(max)");
        getByDbFunc.HasParameter("languageKey").HasStoreType("nvarchar(10)");
    }

    private static void ConfigureGetDefaultFuncSqlServer(ModelBuilder builder)
    {
        var getByDbFunc = builder
            .HasDbFunction(typeof(MultiLanguageFunctions).GetMethod(nameof(MultiLanguageFunctions.GetFirst))!)
            .HasName(Helper.FunctionName(nameof(MultiLanguageFunctions.GetFirst)))
            .IsBuiltIn(false);
        getByDbFunc.HasParameter("prop").HasStoreType("nvarchar(max)");
    }

    private static void ConfigureGetOrDefaultInFuncSqlServer(ModelBuilder builder)
    {
        var getByDbFunc = builder
            .HasDbFunction(typeof(MultiLanguageFunctions).GetMethod(nameof(MultiLanguageFunctions.GetOrFirstIn))!)
            .HasName(Helper.FunctionName(nameof(MultiLanguageFunctions.GetOrFirstIn)))
            .IsBuiltIn(false);
        getByDbFunc.HasParameter("prop").HasStoreType("nvarchar(max)");
        getByDbFunc.HasParameter("languageKey").HasStoreType("nvarchar(10)");
    }

    private static void ConfigureContainsInFuncSqlServer(ModelBuilder builder)
    {
        var isExistsDbFunc = builder
            .HasDbFunction(typeof(MultiLanguageFunctions).GetMethod(nameof(MultiLanguageFunctions.ContainsIn))!)
            .HasName(Helper.FunctionName(nameof(MultiLanguageFunctions.ContainsIn)))
            .IsBuiltIn(false);
        isExistsDbFunc.HasParameter("prop").HasStoreType("nvarchar(max)");
        isExistsDbFunc.HasParameter("languageKey").HasStoreType("nvarchar(10)");
    }

    #endregion
}
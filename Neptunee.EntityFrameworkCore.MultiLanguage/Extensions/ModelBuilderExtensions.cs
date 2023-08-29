using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Neptunee.EntityFrameworkCore.MultiLanguage.Exceptions;
using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Neptunee.EntityFrameworkCore.MultiLanguage.Extensions;

public static class ModelBuilderExtensions
{
    public static void ConfigureMultiLanguage(this ModelBuilder builder, DatabaseFacade database)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var propertyInfo in entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(MultiLanguageProperty)).ToList())
            {
                if (string.Equals(database.ProviderName!, ProviderNames.PostgreSql, StringComparison.CurrentCultureIgnoreCase))
                {
                    builder
                        .Entity(entityType.ClrType)
                        .Property(propertyInfo.Name)
                        .HasColumnType("jsonb");
                }
                else
                {
                    throw new ProviderNotSupportedException(database.ProviderName!);
                }
            }
        }

        ConfigureGetInFunc(builder);
        ConfigureGetDefaultFunc(builder);
        ConfigureGetOrDefaultInFunc(builder);
        ConfigureContainsInFunc(builder);
    }

    private static void ConfigureGetInFunc(ModelBuilder builder)
    {
        var getByDbFunc = builder
            .HasDbFunction(typeof(MultiLanguageFunctions).GetMethod(nameof(MultiLanguageFunctions.GetIn))!)
            .HasName(Helper.FunctionName(nameof(MultiLanguageFunctions.GetIn)))
            .IsBuiltIn(false);
        getByDbFunc.HasParameter("prop").HasStoreType("jsonb");
        getByDbFunc.HasParameter("languageKey").HasStoreType("text");
    }   
    
    private static void ConfigureGetDefaultFunc(ModelBuilder builder)
    {
        var getByDbFunc = builder
            .HasDbFunction(typeof(MultiLanguageFunctions).GetMethod(nameof(MultiLanguageFunctions.GetDefault))!)
            .HasName(Helper.FunctionName(nameof(MultiLanguageFunctions.GetDefault)))
            .IsBuiltIn(false);
        getByDbFunc.HasParameter("prop").HasStoreType("jsonb");
    }

    private static void ConfigureGetOrDefaultInFunc(ModelBuilder builder)
    {
        var getByDbFunc = builder
            .HasDbFunction(typeof(MultiLanguageFunctions).GetMethod(nameof(MultiLanguageFunctions.GetOrDefaultIn))!)
            .HasName(Helper.FunctionName(nameof(MultiLanguageFunctions.GetOrDefaultIn)))
            .IsBuiltIn(false);
        getByDbFunc.HasParameter("prop").HasStoreType("jsonb");
        getByDbFunc.HasParameter("languageKey").HasStoreType("text");
    }

    private static void ConfigureContainsInFunc(ModelBuilder builder)
    {
        var isExistsDbFunc = builder
            .HasDbFunction(typeof(MultiLanguageFunctions).GetMethod(nameof(MultiLanguageFunctions.ContainsIn))!)
            .HasName(Helper.FunctionName(nameof(MultiLanguageFunctions.ContainsIn)))
            .IsBuiltIn(false);
        isExistsDbFunc.HasParameter("prop").HasStoreType("jsonb");
        isExistsDbFunc.HasParameter("languageKey").HasStoreType("text");
    }
}
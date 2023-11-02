using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Neptunee.EntityFrameworkCore.MultiLanguage.Converters;

public class MultiLanguagePropertyValueConverter : ValueConverter<MultiLanguageProperty, string>
{
    /// <inheritdoc />
    public MultiLanguagePropertyValueConverter() : base(
        v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
        v => JsonSerializer.Deserialize<MultiLanguageProperty>(v, new JsonSerializerOptions())!)
    {
    }
}
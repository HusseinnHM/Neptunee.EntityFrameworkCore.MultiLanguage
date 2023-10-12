using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Neptunee.EntityFrameworkCore.MultiLanguage.Converters;

public class MultiLanguagePropertyConverter : ValueConverter<MultiLanguageProperty, string>
{
    /// <inheritdoc />
    public MultiLanguagePropertyConverter() : base(
        v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
        v => JsonSerializer.Deserialize<MultiLanguageProperty>(v, new JsonSerializerOptions())!)
    {
    }
}
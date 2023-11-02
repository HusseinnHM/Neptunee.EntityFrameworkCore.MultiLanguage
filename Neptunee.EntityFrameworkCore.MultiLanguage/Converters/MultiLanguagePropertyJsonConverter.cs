using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Neptunee.EntityFrameworkCore.MultiLanguage.Converters;

/// <inheritdoc />
public class MultiLanguagePropertyJsonConverter : JsonConverter<MultiLanguageProperty>
{
    /// <inheritdoc />
    public override MultiLanguageProperty Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new MultiLanguageProperty(JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, options) ?? new());
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, MultiLanguageProperty value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, typeof(ReadOnlyDictionary<string, string>), options);
    }
}
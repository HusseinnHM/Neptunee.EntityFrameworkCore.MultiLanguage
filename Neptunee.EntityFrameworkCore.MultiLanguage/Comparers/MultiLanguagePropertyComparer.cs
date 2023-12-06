using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Neptunee.EntityFrameworkCore.MultiLanguage.Comparers;

public class MultiLanguagePropertyComparer : ValueComparer<MultiLanguageProperty>
{
    public MultiLanguagePropertyComparer() : base(
        (l, r) => JsonSerializer.Serialize<MultiLanguageProperty>(l, new JsonSerializerOptions()) == JsonSerializer.Serialize<MultiLanguageProperty>(r, new JsonSerializerOptions()),
        v => v.GetHashCode(),
        v => JsonSerializer.Deserialize<MultiLanguageProperty>(JsonSerializer.Serialize(v, new JsonSerializerOptions()), new JsonSerializerOptions()!)!)
    {
    }
}
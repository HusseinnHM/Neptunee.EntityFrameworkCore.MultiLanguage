using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Neptunee.EntityFrameworkCore.MultiLanguage.Converters;
using Neptunee.EntityFrameworkCore.MultiLanguage.Exceptions;

namespace Neptunee.EntityFrameworkCore.MultiLanguage.Types;

[JsonConverter(typeof(MultiLanguagePropertyJsonConverter))]
public class MultiLanguageProperty : ReadOnlyDictionary<string, string>
{
    public MultiLanguageProperty() : base(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase))
    {
    }

    public MultiLanguageProperty(IDictionary<string, string> dictionary) : base(new Dictionary<string, string>(dictionary, StringComparer.OrdinalIgnoreCase))
    {
    }

    public MultiLanguageProperty(string languageKey, string value) : this()
    {
        Upsert(languageKey, value);
    }
    


    public void Upsert(string languageKey, string value)
    {
        languageKey = languageKey.ToLower();
        if (!Dictionary.TryAdd(languageKey, value))
        {
            Dictionary[languageKey] = value;
        }
    }

    public void Modify(string languageKey, string value)
    {
        if (!TryModify(languageKey, value))
        {
            throw new KeyNotFoundException();
        }
    }

    public bool TryModify(string languageKey, string value)
    {
        languageKey = languageKey.ToLower();
        if (!ContainsIn(languageKey))
        {
            return false;
        }

        Dictionary[languageKey] = value;
        return true;
    }

    public void Remove(string languageKey)
    {
        if (Dictionary.Count == 1)
        {
            throw new CannotRemoveDefaultLanguageKeyException();
        }

        Dictionary.Remove(languageKey);
    }


    internal string GetIn(string languageKey)
        => this[languageKey];

    internal string GetFirst()
        => Values.First();

    internal string GetOrFirst(string languageKey)
        => TryGetValue(languageKey, out var value) ? value : GetFirst();

    internal bool ContainsIn(string languageKey)
        => ContainsKey(languageKey);
}
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Neptunee.EntityFrameworkCore.MultiLanguage.Converters;
using Neptunee.EntityFrameworkCore.MultiLanguage.Exceptions;

namespace Neptunee.EntityFrameworkCore.MultiLanguage.Types;

[JsonConverter(typeof(MultiLanguagePropertyJsonConverter))]
public class MultiLanguageProperty : ReadOnlyDictionary<string, string>
{
    internal MultiLanguageProperty(IDictionary<string, string> dictionary) : base(new Dictionary<string, string>(dictionary, StringComparer.OrdinalIgnoreCase))
    {
    }

    public MultiLanguageProperty(string value) : this(new Dictionary<string, string>())
    {
        Upsert(LanguageKey.Default, value);
    }

    public void Upsert(LanguageKey languageKey, string value)
    {
        if (!Dictionary.TryAdd(languageKey, value))
        {
            Dictionary[languageKey] = value;
        }
    }

    public void Modify(LanguageKey languageKey, string value)
    {
        if (!TryModify(languageKey, value))
        {
            throw new KeyNotFoundException();
        }
    }

    public bool TryModify(LanguageKey languageKey, string value)
    {
        if (!ContainsIn(languageKey))
        {
            return false;
        }

        Dictionary[languageKey] = value;
        return true;
    }

    public void Remove(LanguageKey languageKey)
    {
        if (languageKey == LanguageKey.Default)
        {
            throw new CannotRemoveDefaultLanguageKeyException();
        }

        Dictionary.Remove(languageKey);
    }


    internal string GetIn(string languageKey)
        => this[languageKey];

    internal string GetDefault()
        => this[LanguageKey.Default];

    internal string GetOrDefaultIn(string languageKey)
        => TryGetValue(languageKey, out var value) ? value : GetDefault();

    internal bool ContainsIn(string languageKey)
        => ContainsKey(languageKey);
}
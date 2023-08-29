using System.Text.Json.Serialization;
using Neptunee.EntityFrameworkCore.MultiLanguage.Exceptions;

namespace Neptunee.EntityFrameworkCore.MultiLanguage.Types;

public class MultiLanguageProperty : Dictionary<string, string>
{
    [JsonConstructor]
    public MultiLanguageProperty() : base(StringComparer.OrdinalIgnoreCase)
    {
    }

    public MultiLanguageProperty(string value) : this()
    {
        Upsert(LanguageKey.Default, value);
    }


    public new string this[string key] => base[key];

    public void Upsert(LanguageKey languageKey, string value)
    {
        if (!base.TryAdd(languageKey, value))
        {
            base[languageKey] = value;
        }
    }

    public void Modify(LanguageKey languageKey, string value)
    {
        if (!TryModify(languageKey,value))
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

        base[languageKey] = value;
        return true;
    }

    public new void Remove(string languageKey)
    {
        if (languageKey == LanguageKey.Default)
        {
            throw new CannotRemoveDefaultLanguageKeyException();
        }

        base.Remove(languageKey);
    }

    [Obsolete($"Use {nameof(Upsert)}", true)]
    public new void Add(string languageKey, string value)
    {
    }

    [Obsolete($"Use {nameof(Upsert)}", true)]
    public new bool TryAdd(string languageKey, string value) => false;

    internal string GetIn(string languageKey)
        => this[languageKey];

    internal string GetDefault()
        => this[LanguageKey.Default];

    internal string GetOrDefaultIn(string languageKey)
        => TryGetValue(languageKey, out var value) ? value : GetDefault();

    internal bool ContainsIn(string languageKey)
        => ContainsKey(languageKey);
}
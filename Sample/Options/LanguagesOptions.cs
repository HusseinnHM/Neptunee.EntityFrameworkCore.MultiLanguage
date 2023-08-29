using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Sample.Options;

public class LanguagesOptions
{
    private LanguagesOptions()
    {
    }

    private string Default { get; set; }
    private string[] Other { get; set; }


    public static LanguagesOptions Create() => new();

    public LanguagesOptions SetDefault(string @default)
    {
        Default = @default;
        return this;
    }

    public LanguagesOptions SetOther(params string[] other)
    {
        Other = other;
        return this;
    }

    public bool TryParsLanguageKey(string language, out LanguageKey? languageKey)
    {
        if (Validate(language))
        {
            languageKey = language == string.Empty || language.Equals(Default, StringComparison.OrdinalIgnoreCase)
                ? LanguageKey.Default
                : LanguageKey.Pars(language);
            return true;
        }

        languageKey = null;
        return false;
    }

    private bool Validate(string language)
    {
        return language == string.Empty ||
               Default.Equals(language, StringComparison.OrdinalIgnoreCase) ||
               Other.Contains(language, StringComparer.OrdinalIgnoreCase);
    }
}
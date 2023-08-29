using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Neptunee.EntityFrameworkCore.MultiLanguage;

public static class MultiLanguageFunctions
{
    public static string GetIn(this MultiLanguageProperty prop, string languageKey)
        =>prop.GetIn(languageKey); 
    
    public static string GetDefault(this MultiLanguageProperty prop)
        =>prop.GetDefault();
    public static string GetOrDefaultIn(this MultiLanguageProperty prop, string languageKey)
        => prop.GetOrDefaultIn(languageKey);

    public static bool ContainsIn(this MultiLanguageProperty prop, string languageKey)
        => prop.ContainsIn(languageKey);
}

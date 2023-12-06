using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Neptunee.EntityFrameworkCore.MultiLanguage;

public static class MultiLanguageFunctions
{
    public static string GetIn(this MultiLanguageProperty prop, string languageKey)
        =>prop.GetIn(languageKey); 
    
    public static string GetFirst(this MultiLanguageProperty prop)
        =>prop.GetFirst();
    public static string GetOrFirstIn(this MultiLanguageProperty prop, string languageKey)
        => prop.GetOrFirst(languageKey);

    public static bool ContainsIn(this MultiLanguageProperty prop, string languageKey)
        => prop.ContainsIn(languageKey);
}

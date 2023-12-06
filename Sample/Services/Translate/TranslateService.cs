using Neptunee.EntityFrameworkCore.MultiLanguage;
using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Sample.Services;

public class TranslateService : ITranslateService
{
    
    public void Translate(params MultiLanguageProperty[] properties)
    {
        foreach (var property in properties)
        {
            var defaultValue = property.GetFirst();
            foreach (var languageKey in SmapleLanguages.List)
            {
                property.Upsert(languageKey, defaultValue + " " + languageKey);
            }
        }
    }
}
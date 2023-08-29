using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Sample.Services;

public interface ITranslateService
{
    void Translate(params MultiLanguageProperty[] properties);
}
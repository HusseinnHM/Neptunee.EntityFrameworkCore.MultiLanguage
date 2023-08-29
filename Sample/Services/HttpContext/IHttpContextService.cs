using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Sample.Services.HttpContext;

public interface IHttpContextService
{
    LanguageKey GetLanguageKey();
}
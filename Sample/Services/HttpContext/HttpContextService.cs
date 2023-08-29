using Microsoft.Extensions.Options;
using Neptunee.EntityFrameworkCore.MultiLanguage.Types;
using Sample.Options;

namespace Sample.Services.HttpContext;

public class HttpContextService : IHttpContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly LanguagesOptions _languagesOptions;


    public HttpContextService(IHttpContextAccessor httpContextAccessor, LanguagesOptions languagesOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _languagesOptions = languagesOptions;
    }

    public LanguageKey GetLanguageKey()
    {
        var lang = _httpContextAccessor.HttpContext?.Request.Headers["language"].ToString() ?? string.Empty;

        return _languagesOptions.TryParsLanguageKey(lang, out var languageKey)
            ? languageKey!
            : throw new Exception("Header with key language invalid");
    }
}
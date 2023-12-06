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

    public string GetLanguageKey()
    {
        var languageKey = _httpContextAccessor.HttpContext?.Request.Headers["language"].ToString() ?? string.Empty;

        return _languagesOptions.Validate(languageKey)
            ? languageKey!
            : throw new Exception("Header with key language invalid");
    }
}
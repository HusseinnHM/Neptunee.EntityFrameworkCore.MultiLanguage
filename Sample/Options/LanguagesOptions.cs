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

    public bool Validate(string language)
    {
        return
            Default.Equals(language, StringComparison.OrdinalIgnoreCase) ||
            Other.Contains(language, StringComparer.OrdinalIgnoreCase);
    }
}
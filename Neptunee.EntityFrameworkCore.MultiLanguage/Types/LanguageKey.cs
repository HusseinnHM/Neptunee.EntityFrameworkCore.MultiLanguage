namespace Neptunee.EntityFrameworkCore.MultiLanguage.Types;

public record LanguageKey(string Value)
{
    public static readonly LanguageKey Default = new(string.Empty);
    public static LanguageKey Pars(string value) => new(value);
    public string Value { get; } = Value.ToLower();

    public static implicit operator string(LanguageKey languageKey) => languageKey.Value;

    public override string ToString() => Value;
}
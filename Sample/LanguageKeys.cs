using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

public static class SmapleLanguages
{
    public static IEnumerable<LanguageKey> List => new[] { Ar, Fr };

    public static readonly LanguageKey En = LanguageKey.Default;
    public static readonly LanguageKey Ar = new(nameof(Ar));
    public static readonly LanguageKey Fr = new(nameof(Fr));

}
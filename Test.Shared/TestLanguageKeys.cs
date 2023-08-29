using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Test.Shared;

public static class TestLanguageKeys
{
    public static readonly LanguageKey En = LanguageKey.Default;
    public static readonly LanguageKey Ar = new(nameof(Ar));
    public static readonly LanguageKey Fr = new(nameof(Fr));
    public static readonly LanguageKey UnKnown = new(nameof(UnKnown));
}
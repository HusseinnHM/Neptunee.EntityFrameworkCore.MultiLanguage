namespace Neptunee.EntityFrameworkCore.MultiLanguage.Exceptions;

internal class CannotRemoveDefaultLanguageKeyException : Exception
{
    internal CannotRemoveDefaultLanguageKeyException() : base("Cannot Remove Default LanguageKey")
    {
    }
}
namespace Neptunee.EntityFrameworkCore.MultiLanguage.Exceptions;

internal class ProviderNotSupportedException : Exception
{
    internal ProviderNotSupportedException(string providerName) : base($"Provider {providerName} is Not Supported")
    {
    }
}
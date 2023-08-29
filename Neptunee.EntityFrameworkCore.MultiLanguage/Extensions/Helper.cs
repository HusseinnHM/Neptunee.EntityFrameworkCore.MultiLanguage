namespace Neptunee.EntityFrameworkCore.MultiLanguage.Extensions;

internal static class Helper
{
    internal static string FunctionName(string nameOfMethod) => ("multiLan" + nameOfMethod).ToLower();

}
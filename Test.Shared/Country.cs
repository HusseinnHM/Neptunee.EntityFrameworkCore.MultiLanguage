using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Test.Shared;

public class Country
{
    public Guid Id { get; set; }
    public MultiLanguageProperty Name { get; set; }
}
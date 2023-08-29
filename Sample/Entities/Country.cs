using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Sample.Entities;

public class Country
{
    private Country()
    {
        
    }
    public Country(string name)
    {
        Name = new (name);
    }

    public Guid Id { get; set; }
    public MultiLanguageProperty Name { get; set; }
}
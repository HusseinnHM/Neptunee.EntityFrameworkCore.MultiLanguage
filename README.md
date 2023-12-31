# Neptunee.EntityFrameworkCore.MultiLanguage

![](https://img.shields.io/nuget/dt/Neptunee.EntityFrameworkCore.MultiLanguage)
[![](https://img.shields.io/nuget/v/Neptunee.EntityFrameworkCore.MultiLanguage)](https://www.nuget.org/packages/Neptunee.EntityFrameworkCore.MultiLanguage)
![](https://img.shields.io/badge/Unit%20Test-16%20Passed-green)

Working with multi-language database efficiently using EF Core.

<p align="center">
<img width="23%" src="icon.png"  alt="icon"/>

## Supports
- PostgreSQL
- SQL Server

## How It's Work ?

***Write :*** 
 store all translations in the same colum as Json
```json
{
  "language": "value"
}
```
For default language the key is always "" (empty string).

***Read :*** 
 Using custom sql functions and call them from EF Core as static methods you can get The value in the language you want simply.

[More details about create custom sql functions](https://www.linkedin.com/posts/husseinnhm_how-to-create-custom-sql-functions-and-use-activity-7096897369614540800-5hyH)

## Setup
- You should install the NuGet package :
```
dotnet add package Neptunee.EntityFrameworkCore.MultiLanguage
```
- Registration :
```csharp
builder.Services.AddMultiLanguage<SampleDbContext>();
```
- Use ```MultiLanguageProperty``` to define props in entities/columns in tables :
```csharp
public class Entity : BaseEntity
{
    public MultiLanguageProperty Prop { get; set; }
}
```
- in ```DbContext``` override OnModelCreating to Configure ```MultiLanguageProperty``` :
```csharp
public class SampleDbContext: DbContext
{
    /// ctors , DbSets ...
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureMultiLanguage(Database);
        base.OnModelCreating(modelBuilder);
    }
}
```
## Using

***Write :***
```csharp
entity.Prop = new MultiLanguageProperty("defulte language","pew pew");
entity.Prop.Upsert("fr", "péw péw");
context.Add(entity);
await context.SaveChangesAsync();
```

***Read :***
```csharp
context.Entities
       .AsNoTracking()
       .Select(e => new
       {
           Id = e.Id,
           GetIn = e.Prop.GetIn(languageKey),
           GetOrFirstIn = e.Prop.GetOrFirst(languageKey),
           ContainsIn = e.Prop.ContainsIn(languageKey)
       });
```
**SQL Query :** 
```sql
-- @languageKey='fr'

SELECT e."Id",
    MultiLanGetIn(e."Name", @languageKey) AS "GetIn",
    MultilanGetOrFirstIn(e."Name", @languageKey) AS "GetOrFirst",
    MultilanContainsIn(e."Name", @languageKey) AS "ContainsIn"
FROM "Entities" AS e;
```

## More
- [How to create custom SQL functions and use it in EF Core](https://www.linkedin.com/posts/husseinnhm_how-to-create-custom-sql-functions-and-use-activity-7096897369614540800-5hyH)
- [PostgreSQL scripts for the custom SQL functions](https://github.com/HusseinnHM/Neptunee.EntityFrameworkCore.MultiLanguage/blob/master/Neptunee.EntityFrameworkCore.MultiLanguage/HostedServices/CreateMultiLanguageDbFunctions.cs#L68).
- [Sql Server scripts for the custom SQL functions](https://github.com/HusseinnHM/Neptunee.EntityFrameworkCore.MultiLanguage/blob/master/Neptunee.EntityFrameworkCore.MultiLanguage/HostedServices/CreateMultiLanguageDbFunctions.cs#L123).
- [Sample](https://github.com/HusseinnHM/Neptunee.EntityFrameworkCore.MultiLanguage/tree/master/Sample)

using Microsoft.EntityFrameworkCore;
using Neptunee.EntityFrameworkCore.MultiLanguage.DependencyInjection;
using Sample;
using Sample.DbContexts;
using Sample.Options;
using Sample.Services;
using Sample.Services.HttpContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.OperationFilter<LanguageKeyFilter>());

builder.Services
    .AddHttpContextAccessor()
    .AddTransient<IHttpContextService, HttpContextService>()
    .AddTransient<ITranslateService, TranslateService>()
    .AddDbContext<SampleDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddMultiLanguage<SampleDbContext>();

builder.Services.AddSingleton(_ => LanguagesOptions.Create().SetDefault("en").SetOther("ar", "fr"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

using var serviceScope = app.Services.CreateScope();

var context = serviceScope.ServiceProvider.GetRequiredService<SampleDbContext>();
await context.Database.MigrateAsync();

app.Run();
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neptunee.EntityFrameworkCore.MultiLanguage;
using Sample.DbContexts;
using Sample.Entities;
using Sample.Requests;
using Sample.Services;
using Sample.Services.HttpContext;

namespace Sample.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CountryController : ControllerBase
{
    private readonly SampleDbContext _context;
    private readonly IHttpContextService _httpContextService;

    public CountryController(SampleDbContext context,
        IHttpContextService httpContextService)
    {
        _context = context;
        _httpContextService = httpContextService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var languageKey = _httpContextService.GetLanguageKey();

        return Ok(await _context.Countries.Select(c => new
        {
            c.Id,
            Name = c.Name.GetOrFirstIn(languageKey),
        }).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromServices] ITranslateService translateService,[FromBody] AddCountryRequest request)
    {
        var country = new Country(request.Name);
        translateService.Translate(country.Name);
        _context.Add(country);
        await _context.SaveChangesAsync();
        return Ok(new
        {
            country.Id,
            Name = country.Name.GetFirst(),
        });
    }

    [HttpPost]
    public async Task<IActionResult> Modify([FromBody] ModifyCountryRequest request)
    {
        var languageKey = _httpContextService.GetLanguageKey();
        var country = await _context.Countries.Where(c => c.Id == request.Id).FirstAsync();
        country.Name.Upsert(languageKey, request.Name);
        _context.Update(country);
        await _context.SaveChangesAsync();
        return Ok(new
        {
            country.Id,
            Name = country.Name.GetOrFirstIn(languageKey),
        });
    }
}
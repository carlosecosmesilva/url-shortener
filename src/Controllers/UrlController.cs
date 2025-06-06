using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services.Interfaces;

[ApiController]
[Route("[controller]")]
public class UrlController : ControllerBase
{
    private readonly IUrlService _urlService;
    private readonly IHttpContextAccessor _httpContext;

    public UrlController(IUrlService urlService, IHttpContextAccessor httpContext)
    {
        _urlService = urlService;
        _httpContext = httpContext;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShortUrl([FromBody] CreateShortUrlRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var sanitizedUrl = request.Url.Trim();

        if (!sanitizedUrl.StartsWith("http://") && !sanitizedUrl.StartsWith("https://"))
            return BadRequest("Only http and https URLs are allowed.");

        sanitizedUrl = sanitizedUrl.TrimEnd('/');

        var result = await _urlService.CreateShortUrlAsync(sanitizedUrl);
        return Ok(result);
    }

    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToLongUrl(string shortCode)
    {
        var longUrl = await _urlService.GetLongUrlAsync(shortCode);
        return longUrl == null ? NotFound() : RedirectPermanent(longUrl);
    }
}

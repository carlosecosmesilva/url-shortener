using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("")]
public class UrlController : ControllerBase
{
    private readonly IUrlService _urlService;
    private readonly IHttpContextAccessor _httpContext;

    public UrlController(IUrlService urlService, IHttpContextAccessor httpContext)
    {
        _urlService = urlService;
        _httpContext = httpContext;
    }

    [HttpPost("shorten")]
    public async Task<IActionResult> ShortenUrl([FromBody] UrlDto dto)
    {
        var shortCode = await _urlService.ShortenUrlAsync(dto.LongUrl);
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        return Ok(new { short_url = $"{baseUrl}/{shortCode}" });
    }

    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToLongUrl(string shortCode)
    {
        var longUrl = await _urlService.GetLongUrlAsync(shortCode);
        return longUrl == null ? NotFound() : RedirectPermanent(longUrl);
    }
}

using Microsoft.AspNetCore.Mvc;
using UrlShortener.API.Models;
using UrlShortener.API.Services;

namespace UrlShortener.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShortUrlController : ControllerBase
{
    private readonly ShortUrlService _service;

    public ShortUrlController(ShortUrlService service)
    {
        _service = service;
    }

    /// <summary>
    /// Gets all short URLs.
    /// </summary>
    /// <returns>A list of short URLs.</returns>
    /// <response code="200">Returns the list of short URLs.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var urls = await _service.GetAllAsync();
        return Ok(urls);
    }

    /// <summary>
    /// Gets a short URL by its ID.
    /// </summary>
    /// <param name="id">The ID of the short URL.</param>
    /// <returns>The short URL if found, otherwise NotFound.</returns>
    /// <response code="200">Returns the short URL.</response>
    /// <response code="404">If the short URL is not found.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var url = await _service.GetByIdAsync(id);
        return url is null ? NotFound() : Ok(url);
    }

    /// <summary>
    /// Redirects to the original URL using the short code.
    /// </summary>
    /// <param name="code">The short code of the URL.</param>
    /// <returns>Redirects to the original URL if found, otherwise NotFound.</returns>
    /// <response code="302">Redirects to the original URL.</response>
    /// <response code="404">If the short URL is not found.</response>
    [HttpGet("r/{code}")]
    public async Task<IActionResult> RedirectToOriginal(string code)
    {
        var url = await _service.GetByCodeAsync(code);
        return url is null ? NotFound() : Redirect(url.OriginalUrl);
    }

    /// <summary>
    /// Creates a new short URL.
    /// </summary>
    /// <param name="originalUrl">The original URL to shorten.</param>
    /// <returns>The created short URL.</returns>
    /// <response code="201">Returns the created short URL.</response>
    /// <response code="400">If the original URL is invalid.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] string originalUrl)
    {
        if (string.IsNullOrWhiteSpace(originalUrl))
            return BadRequest("URL inválida.");

        var shortUrl = await _service.CreateShortUrlAsync(originalUrl);
        return CreatedAtAction(nameof(GetById), new { id = shortUrl.Id }, shortUrl);
    }

    /// <summary>
    /// Updates an existing short URL.
    /// </summary>
    /// <param name="id">The ID of the short URL to update.</param>
    /// <param name="updatedUrl">The updated short URL data.</param>
    /// <returns>No content if successful.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ShortUrl updatedUrl)
    {
        if (id != updatedUrl.Id)
            return BadRequest();

        await _service.UpdateAsync(updatedUrl);
        return NoContent();
    }

    /// <summary>
    /// Deletes a short URL by its ID.
    /// </summary>
    /// <param name="id">The ID of the short URL to delete.</param>
    /// <returns>No content if successful.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}

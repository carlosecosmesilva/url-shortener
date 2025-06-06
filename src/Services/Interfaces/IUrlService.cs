using UrlShortener.Models;

namespace UrlShortener.Services.Interfaces;

public interface IUrlService
{
    Task<Url> CreateShortUrlAsync(string originalUrl);
    Task<Url?> GetByShortCodeAsync(string shortCode);
    Task<string?> GetLongUrlAsync(string shortCode);
}
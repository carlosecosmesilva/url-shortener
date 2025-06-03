using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Services.Interfaces;
using UrlShortener.Utils;

namespace UrlShortener.Services;

public class UrlService : IUrlService
{
    private readonly ApplicationDbContext _context;
    private const int MaxRetries = 3;

    public UrlService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Url> CreateShortUrlAsync(string originalUrl)
    {
        // Normalize the URL
        if (!originalUrl.StartsWith("http://") && !originalUrl.StartsWith("https://"))
        {
            originalUrl = "https://" + originalUrl;
        }

        // Check if URL already exists
        var existingUrl = await _context.Urls.FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl);
        if (existingUrl != null)
        {
            return existingUrl;
        }

        // Create new short URL
        var url = new Url
        {
            OriginalUrl = originalUrl
        };

        // Try to generate a unique short code
        for (int i = 0; i < MaxRetries; i++)
        {
            url.ShortCode = UrlCodeGenerator.GenerateShortCode(originalUrl + i);
            if (!await _context.Urls.AnyAsync(u => u.ShortCode == url.ShortCode))
            {
                _context.Urls.Add(url);
                await _context.SaveChangesAsync();
                return url;
            }
        }

        throw new InvalidOperationException("Could not generate a unique short code after multiple attempts.");
    }

    public async Task<Url?> GetByShortCodeAsync(string shortCode)
    {
        var url = await _context.Urls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        if (url != null)
        {
            url.LastAccessedAt = DateTime.UtcNow;
            url.ClickCount++;
            await _context.SaveChangesAsync();
        }
        return url;
    }
}

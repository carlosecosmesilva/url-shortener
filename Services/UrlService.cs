using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Services.Interfaces;

namespace UrlShortener.Services;

public class UrlService : IUrlService
{
    private readonly ApplicationDbContext _context;

    public UrlService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Url> CreateShortUrlAsync(string originalUrl)
    {
        var url = new Url
        {
            OriginalUrl = originalUrl,
            ShortCode = GenerateShortCode(originalUrl)
        };

        _context.Urls.Add(url);
        await _context.SaveChangesAsync();
        return url;
    }

    public async Task<Url?> GetByShortCodeAsync(string shortCode)
    {
        return await _context.Urls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
    }

    private static string GenerateShortCode(string url)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(url + DateTime.UtcNow.Ticks));
        return Convert.ToBase64String(hash).Replace("/", "_").Replace("+", "-").Substring(0, 8);
    }
}

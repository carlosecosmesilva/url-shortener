using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Data;
using UrlShortener.API.Models;

namespace UrlShortener.API.Repositories;

public class ShortUrlRepository : IShortUrlRepository
{
    private readonly AppDbContext _context;

    public ShortUrlRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShortUrl>> GetAllAsync() =>
        await _context.ShortUrls.ToListAsync();

    public async Task<ShortUrl?> GetByCodeAsync(string code) =>
        await _context.ShortUrls.FirstOrDefaultAsync(s => s.ShortCode == code);

    public async Task<ShortUrl?> GetByIdAsync(int id) =>
        await _context.ShortUrls.FindAsync(id);

    public async Task<ShortUrl> CreateAsync(ShortUrl shortUrl)
    {
        _context.ShortUrls.Add(shortUrl);
        await _context.SaveChangesAsync();
        return shortUrl;
    }

    public async Task UpdateAsync(ShortUrl shortUrl)
    {
        _context.ShortUrls.Update(shortUrl);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var shortUrl = await _context.ShortUrls.FindAsync(id);
        if (shortUrl != null)
        {
            _context.ShortUrls.Remove(shortUrl);
            await _context.SaveChangesAsync();
        }
    }

    public async Task IncrementAccessCountAsync(ShortUrl shortUrl)
    {
        shortUrl.AccessCount++;
        await _context.SaveChangesAsync();
    }
}
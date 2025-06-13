using UrlShortener.API.Models;

namespace UrlShortener.API.Repositories;

public interface IShortUrlRepository
{
    Task<IEnumerable<ShortUrl>> GetAllAsync();
    Task<ShortUrl?> GetByCodeAsync(string code);
    Task<ShortUrl?> GetByIdAsync(int id);
    Task<ShortUrl> CreateAsync(ShortUrl shortUrl);
    Task UpdateAsync(ShortUrl shortUrl);
    Task DeleteAsync(int id);
    Task IncrementAccessCountAsync(ShortUrl shortUrl);
}

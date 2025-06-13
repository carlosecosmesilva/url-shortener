using UrlShortener.API.Models;
using UrlShortener.API.Repositories;

namespace UrlShortener.API.Services;

public class ShortUrlService
{
    private readonly IShortUrlRepository _repository;

    public ShortUrlService(IShortUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShortUrl> CreateShortUrlAsync(string originalUrl)
    {
        var shortCode = Guid.NewGuid().ToString()[..8]; // código aleatório
        var shortUrl = new ShortUrl
        {
            OriginalUrl = originalUrl,
            ShortCode = shortCode
        };

        return await _repository.CreateAsync(shortUrl);
    }

    public async Task<ShortUrl?> GetByCodeAsync(string code)
    {
        var url = await _repository.GetByCodeAsync(code);
        if (url != null)
        {
            await _repository.IncrementAccessCountAsync(url);
        }
        return url;
    }

    public Task<IEnumerable<ShortUrl>> GetAllAsync() => _repository.GetAllAsync();
    public Task<ShortUrl?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
    public Task UpdateAsync(ShortUrl url) => _repository.UpdateAsync(url);
    public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
}

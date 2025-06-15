using MassTransit;
using UrlShortener.API.Models;
using UrlShortener.API.Repositories;
using UrlShortener.API.Messaging.Contracts;

namespace UrlShortener.API.Services;

public class ShortUrlService(IShortUrlRepository repository, IPublishEndpoint publishEndpoint, IConfiguration configuration)
{
    private readonly IShortUrlRepository _repository = repository;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly string _baseUrl = configuration["ShortUrl:BaseUrl"] ?? "http://localhost:5142/r/";

    public async Task<ShortUrl> CreateShortUrlAsync(string originalUrl)
    {
        try
        {
            var shortCode = Guid.NewGuid().ToString()[..8];
            var shortUrl = new ShortUrl
            {
                OriginalUrl = originalUrl,
                ShortCode = shortCode
            };

            var saved = await _repository.CreateAsync(shortUrl);

            // Monta a URL curta
            saved.ShortUrlValue = _baseUrl + saved.ShortCode;

            await _publishEndpoint.Publish<IShortUrlCreated>(new
            {
                saved.Id,
                saved.OriginalUrl,
                saved.ShortCode
            });

            return saved;
        }
        catch (System.Exception ex)
        {
            await _publishEndpoint.Publish<IShortUrlCreationFailed>(new
            {
                OriginalUrl = originalUrl,
                Reason = ex.Message
            });
            throw;
        }
    }

    public async Task<ShortUrl?> GetByCodeAsync(string code)
    {
        var url = await _repository.GetByCodeAsync(code);
        if (url != null)
        {
            url.ShortUrlValue = _baseUrl + url.ShortCode;
            await _repository.IncrementAccessCountAsync(url);
        }
        return url;
    }

    public async Task<IEnumerable<ShortUrl>> GetAllAsync()
    {
        var urls = await _repository.GetAllAsync();
        foreach (var url in urls)
        {
            url.ShortUrlValue = _baseUrl + url.ShortCode;
        }
        return urls;
    }

    public async Task<ShortUrl?> GetByIdAsync(int id)
    {
        var url = await _repository.GetByIdAsync(id);
        if (url != null)
            url.ShortUrlValue = _baseUrl + url.ShortCode;
        return url;
    }
    public Task UpdateAsync(ShortUrl url) => _repository.UpdateAsync(url);
    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await _repository.DeleteAsync(id);

        if (deleted)
        {
            await _publishEndpoint.Publish<IShortUrlDeleted>(new
            {
                Id = id,
                ShortCode = (await _repository.GetByIdAsync(id))?.ShortCode ?? string.Empty
            });
        }

        return deleted;
    }
}

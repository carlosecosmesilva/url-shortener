using Xunit;
using UrlShortener.Services;
using UrlShortener.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using UrlShortener.Models;

/// <summary>
/// Contains unit tests for the UrlService class.
/// </summary>
public class UrlServiceTests
{
    /// <summary>
    /// Tests that CreateShortUrlAsync returns a valid short URL for a given original URL.
    /// </summary>
    [Fact]
    public async Task CreateShortUrlAsync_ReturnsShortUrl()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "UrlShortenerTestDb")
            .Options;
        using var context = new ApplicationDbContext(options);
        var service = new UrlService(context);

        var url = await service.CreateShortUrlAsync("https://example.com");

        Assert.NotNull(url);
        Assert.Equal("https://example.com", url.OriginalUrl);
        Assert.False(string.IsNullOrEmpty(url.ShortCode));
    }
}
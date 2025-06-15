namespace UrlShortener.API.Messaging.Contracts;

public class IShortUrlCreated
{
    public int Id { get; }
    public string OriginalUrl { get; }
    public string ShortCode { get; }
}
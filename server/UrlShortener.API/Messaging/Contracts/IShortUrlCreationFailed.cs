namespace UrlShortener.API.Messaging.Contracts;

public interface IShortUrlCreationFailed
{
    string OriginalUrl { get; }
    string Reason { get; }
}
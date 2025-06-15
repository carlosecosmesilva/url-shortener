namespace UrlShortener.API.Messaging.Contracts;

public interface IShortUrlDeleted
{
    int Id { get; }
    string ShortCode { get; }
}
using MassTransit;
using UrlShortener.API.Messaging.Contracts;

namespace UrlShortener.API.Messaging.Consumers;

public class ShortUrlCreatedConsumer : IConsumer<IShortUrlCreated>
{
    public async Task Consume(ConsumeContext<IShortUrlCreated> context)
    {
        var message = context.Message;
        // Here you can handle the message, e.g., log it or update a database
        Console.WriteLine($"Short URL Created: Id={message.Id}, OriginalUrl={message.OriginalUrl}, ShortCode={message.ShortCode}");
        
        // Simulate some processing delay
        await Task.Delay(1000);
    }
}
using MassTransit;
using UrlShortener.API.Messaging.Contracts;

namespace UrlShortener.API.Messaging.Consumers;

public class ShortUrlDeletedConsumer : IConsumer<IShortUrlDeleted>
{
    public Task Consume(ConsumeContext<IShortUrlDeleted> context)
    {
        Console.WriteLine($"[RabbitMQ] URL deletada: {context.Message.ShortCode} (ID: {context.Message.Id})");
        return Task.CompletedTask;
    }
}
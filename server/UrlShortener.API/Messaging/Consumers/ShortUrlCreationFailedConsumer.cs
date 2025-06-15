using MassTransit;
using UrlShortener.API.Messaging.Contracts;

namespace UrlShortener.API.Messaging.Consumers;

public class ShortUrlCreationFailedConsumer : IConsumer<IShortUrlCreationFailed>
{
    public Task Consume(ConsumeContext<IShortUrlCreationFailed> context)
    {
        Console.WriteLine($"[RabbitMQ] Falha ao criar URL: {context.Message.OriginalUrl} | Motivo: {context.Message.Reason}");
        return Task.CompletedTask;
    }
}
namespace UrlShortener.API.Messaging.Messages
{
    public class LoginEvent
    {
        public string Username { get; set; } = string.Empty;
        public bool Success { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Reason { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace UrlShortener.API.DTOs;

public class ShortUrlRequest
{
    [Required]
    public string OriginalUrl { get; set; } = string.Empty;
}
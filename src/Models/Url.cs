using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models;

public class Url
{

    [Required]
    [Url]
    public string OriginalUrl { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string ShortCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastAccessedAt { get; set; }

    public int ClickCount { get; set; } = 0;
}
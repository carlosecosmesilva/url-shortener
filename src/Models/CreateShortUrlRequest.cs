using System.ComponentModel.DataAnnotations;

public class CreateShortUrlRequest
{
    [Required(ErrorMessage = "URL is required.")]
    [Url(ErrorMessage = "Invalid URL format.")]
    [MaxLength(2048, ErrorMessage = "URL is too long.")]
    public string Url { get; set; } = string.Empty;
}
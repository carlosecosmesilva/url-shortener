using System.Security.Cryptography;
using System.Text;
namespace UrlShortener.Utils;

public static class UrlCodeGenerator
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateShortCode(string input)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input + DateTime.UtcNow.Ticks));
        var chars = Alphabet.ToCharArray();
        var hashLength = Math.Min(hash.Length, 6); // Limit to 6 bytes for shorter codes
        var result = new StringBuilder();

        for (int i = 0; i < 6; i++) // Generate 6-character code
        {
            result.Append(chars[hash[i] % hashLength]);
        }

        return result.ToString();
    }
}

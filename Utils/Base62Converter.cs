using System.Text;

public static class Base62Converter
{
    private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string Encode(int number)
    {
        if (number == 0) return "0";
        var base62 = new StringBuilder();
        while (number > 0)
        {
            base62.Insert(0, Alphabet[number % 62]);
            number /= 62;
        }
        return base62.ToString();
    }
}

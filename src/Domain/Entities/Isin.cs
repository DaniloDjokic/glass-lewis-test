
namespace Domain.Entities;

public class Isin
{
    public string Value { get; init; }

    public Isin(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("ISIN value cannot be null or empty.", nameof(value));
        }

        var firstTwoChars = value.Substring(0, 2).ToUpperInvariant();

        if (!firstTwoChars.All(char.IsLetter))
        {
            throw new ArgumentException("ISIN must start with two letters.", nameof(value));
        }

        Value = value;
    }
}

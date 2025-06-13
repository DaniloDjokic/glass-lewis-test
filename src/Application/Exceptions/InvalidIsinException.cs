namespace Application.Exceptions;

public class InvalidIsinException : Exception
{
    public InvalidIsinException(string isin)
        : base($"The first two characters of the ISIN must be letters. Provided ISIN: {isin}")
    {
    }

    public InvalidIsinException(string isin, Exception innerException)
        : base($"The first two characters of the ISIN must be letters. Provided ISIN: {isin}", innerException)
    {
    }
}

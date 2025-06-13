namespace Application.Exceptions;

public class DuplicateIsinException : Exception
{
    public DuplicateIsinException(string isin)
        : base($"A company with ISIN {isin} already exists.")
    {
    }

    public DuplicateIsinException(string isin, Exception innerException)
        : base($"A company with ISIN {isin} already exists.", innerException)
    {
    }
}

namespace Application.Exceptions;

public class ValidationException : Exception
{
    public IEnumerable<string> Errors { get; init; }

    public ValidationException(string message, IEnumerable<string> errors) : base(message)
    {
        Errors = errors ?? throw new ArgumentNullException(nameof(errors), "Errors cannot be null");
    }

    public ValidationException(string message, IEnumerable<string> errors, Exception innerException) : base(message, innerException)
    {
        Errors = errors ?? throw new ArgumentNullException(nameof(errors), "Errors cannot be null");
    }
}

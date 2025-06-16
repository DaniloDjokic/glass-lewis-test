namespace Application.Common;

public class ValidationResult
{
    public bool IsValid { get; }
    public string[] Errors { get; } = [];

    public ValidationResult(bool isValid, string[] errors)
    {
        IsValid = isValid;
        Errors = errors;
    }

    public static ValidationResult Success() => new ValidationResult(true, []);
    public static ValidationResult Failure(string[] errors) => new ValidationResult(false, errors);
}

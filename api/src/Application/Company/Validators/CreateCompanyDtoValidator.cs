using Application.Common;
using Application.DTOs;

namespace Application.Validators;

public static class CreateCompanyDtoValidator
{
    public static ValidationResult Validate(CreateCompanyDTO dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            errors.Add("Company name is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Exchange))
        {
            errors.Add("Company exchange is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Ticker))
        {
            errors.Add("Company ticker is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Isin))
        {
            errors.Add("Company isin is required.");
        }

        return errors.Count == 0 ? ValidationResult.Success() : ValidationResult.Failure(errors.ToArray());
    }
}

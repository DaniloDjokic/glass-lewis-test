using Application.DTOs;
using Application.Validators;

namespace Tests.Unit;

[Trait("Category", "Unit")]
public class CompanyValidatorTests
{
    [Fact]
    public void CreateCompanyDtoValidator_EmptyName_ReturnsFailure()
    {
        var createCompanyDto = new CreateCompanyDTO(
            string.Empty,
            "NYSE",
            "TEST",
            "US123456",
            ""
        );

        var result = CreateCompanyDtoValidator.Validate(createCompanyDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void CreateCompanyDtoValidator_EmptyExchange_ReturnsFailure()
    {
        var createCompanyDto = new CreateCompanyDTO(
            "Test",
            string.Empty,
            "TEST",
            "US123456",
            ""
        );

        var result = CreateCompanyDtoValidator.Validate(createCompanyDto);
        Assert.False(result.IsValid);
    }


    [Fact]
    public void CreateCompanyDtoValidator_EmptyTicker_ReturnsFailure()
    {
        var createCompanyDto = new CreateCompanyDTO(
            "Test",
            "Test",
            string.Empty,
            "US123456",
            ""
        );

        var result = CreateCompanyDtoValidator.Validate(createCompanyDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void CreateCompanyDtoValidator_EmptyIsin_ReturnsFailure()
    {
        var createCompanyDto = new CreateCompanyDTO(
            "Test",
            "Test",
            "Test",
            string.Empty,
            ""
        );

        var result = CreateCompanyDtoValidator.Validate(createCompanyDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void CreateCompanyDtoValidator_EmptyWebsiteUrl_ReturnsSuccess()
    {
        var createCompanyDto = new CreateCompanyDTO(
            "Test",
            "Test",
            "Test",
            "US123456",
            ""
        );

        var result = CreateCompanyDtoValidator.Validate(createCompanyDto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void CreateCompanyDtoValidator_ValidData_ReturnsSuccess()
    {
        var createCompanyDto = new CreateCompanyDTO(
            "Test Company",
            "NYSE",
            "TEST",
            "US1234567890",
            "https://www.testcompany.com"
        );

        var result = CreateCompanyDtoValidator.Validate(createCompanyDto);
        Assert.True(result.IsValid);
    }


    [Fact]
    public void UpdateCompanyDtoValidator_EmptyName_ReturnsFailure()
    {
        var updateCompanyDto = new UpdateCompanyDTO(
            string.Empty,
            "NYSE",
            "TEST",
            "US123456",
            ""
        );

        var result = UpdateCompanyDtoValidator.Validate(updateCompanyDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void UpdateCompanyDtoValidator_EmptyExchange_ReturnsFailure()
    {
        var updateCompanyDto = new UpdateCompanyDTO(
            "Test",
            string.Empty,
            "TEST",
            "US123456",
            ""
        );

        var result = UpdateCompanyDtoValidator.Validate(updateCompanyDto);
        Assert.False(result.IsValid);
    }


    [Fact]
    public void UpdateCompanyDtoValidator_EmptyTicker_ReturnsFailure()
    {
        var updateCompanyDto = new UpdateCompanyDTO(
            "Test",
            "Test",
            string.Empty,
            "US123456",
            ""
        );

        var result = UpdateCompanyDtoValidator.Validate(updateCompanyDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void UpdateCompanyDtoValidator_EmptyIsin_ReturnsFailure()
    {
        var updateCompanyDto = new CreateCompanyDTO(
            "Test",
            "Test",
            "Test",
            string.Empty,
            ""
        );

        var result = CreateCompanyDtoValidator.Validate(updateCompanyDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void UpdateCompanyDtoValidator_EmptyWebsiteUrl_ReturnsSuccess()
    {
        var updateCompanyDto = new UpdateCompanyDTO(
            "Test",
            "Test",
            "Test",
            "US123456",
            ""
        );

        var result = UpdateCompanyDtoValidator.Validate(updateCompanyDto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void UpdateCompanyDtoValidator_ValidData_ReturnsSuccess()
    {
        var updateCompanyDto = new UpdateCompanyDTO(
            "Test Company",
            "NYSE",
            "TEST",
            "US1234567890",
            "https://www.testcompany.com"
        );

        var result = UpdateCompanyDtoValidator.Validate(updateCompanyDto);
        Assert.True(result.IsValid);
    }
}

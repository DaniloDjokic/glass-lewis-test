using Application.DTOs;

namespace Application.Services;

public interface ICompanyService
{
    /// <summary>
    /// Retrieves a list of companies.
    /// </summary>
    /// <returns>A list of company DTOs.</returns>
    Task<IEnumerable<CompanyDTO>> GetCompaniesAsync();
}

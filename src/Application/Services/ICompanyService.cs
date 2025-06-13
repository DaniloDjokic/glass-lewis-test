using Application.DTOs;
using Application.Requests;

namespace Application.Services;

public interface ICompanyService
{
    /// <summary>
    /// Retrieves a list of companies.
    /// </summary>
    /// <returns>A list of company DTOs.</returns>
    Task<IEnumerable<CompanyDTO>> GetCompaniesAsync();

    /// <summary>
    /// Retrieves a company by its ID.
    /// /// </summary>
    /// <param name="id">The ID of the company.</param>
    /// <returns>A company DTO if found, otherwise null.</returns>
	Task<CompanyDTO?> GetCompanyByIdAsync(int id);

    /// <summary>
    /// Retrieves a company by its isin.
    /// </summary>
    /// <param name="isin">The isin of the company.</param>
    /// <returns>A company DTO if found, otherwise null.</returns>
	Task<CompanyDTO?> GetCompanyByIsinAsync(string isin);

    /// <summary>
    /// Creates a new company.
    /// </summary>
    /// <param name="createCompanyRequest">The request containing the company data</param>
    /// <returns>The created company Id</returns>
	Task<int> CreateCompanyAsync(CreateCompanyRequest createCompanyRequest);


    /// <summary>
    /// Updates an existing company.
    /// </summary>
    /// <param name="updateCompanyRequest">The request containing the company data</param>
	Task UpdateCompanyAsync(int id, UpdateCompanyRequest updateCompanyRequest);
}

using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface ICompanyRepository
{
    /// <summary>
    /// Retrieves all companies from the repository.
    /// </summary>
    /// <returns>A collection of companies.</returns>
    Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync();

    /// <summary>
    /// Retrieves a company by its ID.
    /// /// </summary>
    /// <param name="id">The ID of the company.</param>
    /// <returns>A company DTO if found, otherwise null.</returns>
	Task<CompanyDTO?> GetCompanyByIdAsync(int id);

    /// <summary>
    /// Checks if a company with the given ID exists in the repository.
    /// </summary>
    /// <param name="id">The ID of the company to check</param>
    /// <returns>A boolean that represents if the company with the given ID exists</returns>
    Task<bool> CompanyExistsAsync(int id);

    /// <summary>
    /// Retrieves a company by its isin.
    /// </summary>
    /// <param name="isin">The isin of the company.</param>
    /// <returns>A company DTO if found, otherwise null.</returns>
	Task<CompanyDTO?> GetCompanyByIsinAsync(string isin);

    /// <summary>
    /// Checks if a company with the given ISIN exists in the repository.
    /// </summary>
    /// <param name="company">The ISIN of the company to check</param>
    /// <returns>A boolean the represents if the company with the given ISIN exists</returns>
    Task<bool> DoesIsinExistAsync(string isin);

    /// <summary>
    /// Creates a new company in the repository.
    /// </summary>
    /// <param name="company">The company to create.</param>
    /// <returns>The ID of the created company.</returns>
    Task<int> CreateCompanyAsync(Company company);

    /// <summary>
    /// Updates an existing company in the repository.
    /// </summary>
    /// <param name="company">The company to update.</param>
	Task UpdateCompanyAsync(int id, Company updatedCompany);
}

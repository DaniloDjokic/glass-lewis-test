namespace Application.Services;

using Application.DTOs;
using Application.Interfaces;
using Application.Requests;

public class CompanyService(ICompanyRepository companyRepository) : ICompanyService
{
    public async Task<IEnumerable<CompanyDTO>> GetCompaniesAsync()
    {
        return await companyRepository.GetAllCompaniesAsync();
    }

    public async Task<CompanyDTO?> GetCompanyByIdAsync(int id)
    {
        return await companyRepository.GetCompanyByIdAsync(id);
    }

    public async Task<CompanyDTO?> GetCompanyByIsinAsync(string isin)
    {
        return await companyRepository.GetCompanyByIsinAsync(isin);
    }

    public async Task<int> CreateCompanyAsync(CreateCompanyRequest companyDto)
    {
        var isinExists = await companyRepository.DoesIsinExistAsync(companyDto.Isin);

        if (isinExists)
        {
            throw new InvalidOperationException($"A company with ISIN {companyDto.Isin} already exists.");
        }

        var isinFirstTwo = companyDto.Isin.Substring(0, 2).ToUpperInvariant();

        if (!isinFirstTwo.All(char.IsLetter))
        {
            throw new ArgumentException("The first two characters of the ISIN must be letters.", nameof(companyDto.Isin));
        }

        var company = CreateCompanyRequest.ToEntity(companyDto);

        return await companyRepository.CreateCompanyAsync(company);
    }

    public async Task UpdateCompanyAsync(int id, UpdateCompanyRequest updateCompanyRequest)
    {
        var company = await companyRepository.GetCompanyByIdAsync(id);
        if (company == null)
        {
            throw new KeyNotFoundException($"Company with ID {id} not found.");
        }

        if (updateCompanyRequest.Isin != company.Isin)
        {
            var isinExists = await companyRepository.DoesIsinExistAsync(updateCompanyRequest.Isin);

            if (isinExists)
            {
                throw new InvalidOperationException($"A company with ISIN {updateCompanyRequest.Isin} already exists.");
            }

            var isinFirstTwo = updateCompanyRequest.Isin.Substring(0, 2).ToUpperInvariant();
            if (!isinFirstTwo.All(char.IsLetter))
            {
                throw new ArgumentException("The first two characters of the ISIN must be letters.", nameof(updateCompanyRequest.Isin));
            }
        }

        var updatedCompany = UpdateCompanyRequest.MapFromRequest(updateCompanyRequest);

        await companyRepository.UpdateCompanyAsync(id, updatedCompany);
    }
}

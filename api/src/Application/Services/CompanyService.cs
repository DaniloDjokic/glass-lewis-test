namespace Application.Services;

using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;

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

    public async Task<int> CreateCompanyAsync(CreateCompanyDTO companyDto)
    {
        await IsValidIsinAsync(companyDto.Isin);

        return await companyRepository.CreateCompanyAsync(companyDto);
    }

    private async Task IsValidIsinAsync(string isin)
    {
        var isinExists = await companyRepository.DoesIsinExistAsync(isin);

        if (isinExists)
        {
            throw new DuplicateIsinException(isin);
        }

        var isinFirstTwo = isin.Substring(0, 2).ToUpperInvariant();

        if (!isinFirstTwo.All(char.IsLetter))
        {
            throw new InvalidIsinException(isin);
        }
    }

    public async Task UpdateCompanyAsync(int id, UpdateCompanyDTO updateCompanyDto)
    {
        var company = await companyRepository.GetCompanyByIdAsync(id);
        if (company == null)
        {
            throw new CompanyNotFoundException(id);
        }

        if (updateCompanyDto.Isin != company.Isin)
        {
            await IsValidIsinAsync(updateCompanyDto.Isin);
        }

        await companyRepository.UpdateCompanyAsync(id, updateCompanyDto);
    }
}

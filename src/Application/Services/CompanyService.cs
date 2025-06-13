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

    //TODO: Clean up this reused logic

    public async Task<int> CreateCompanyAsync(CreateCompanyDTO companyDto)
    {
        var isinExists = await companyRepository.DoesIsinExistAsync(companyDto.Isin);

        if (isinExists)
        {
            throw new DuplicateIsinException(companyDto.Isin);
        }

        var isinFirstTwo = companyDto.Isin.Substring(0, 2).ToUpperInvariant();

        if (!isinFirstTwo.All(char.IsLetter))
        {
            throw new InvalidIsinException(companyDto.Isin);
        }

        return await companyRepository.CreateCompanyAsync(companyDto);
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
            var isinExists = await companyRepository.DoesIsinExistAsync(updateCompanyDto.Isin);

            if (isinExists)
            {
                throw new DuplicateIsinException(updateCompanyDto.Isin);
            }

            var isinFirstTwo = updateCompanyDto.Isin.Substring(0, 2).ToUpperInvariant();
            if (!isinFirstTwo.All(char.IsLetter))
            {
                throw new InvalidIsinException(updateCompanyDto.Isin);
            }
        }

        await companyRepository.UpdateCompanyAsync(id, updateCompanyDto);
    }
}

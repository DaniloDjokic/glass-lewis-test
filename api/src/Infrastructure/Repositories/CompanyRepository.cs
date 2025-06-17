using Application.DTOs;
using Application.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class CompanyRepository(ApplicationDbContext dbContext) : ICompanyRepository
{
    public async Task<IReadOnlyCollection<CompanyDTO>> GetAllCompaniesAsync()
    {
        return await dbContext.Companies
            .Select(c => CompanyDTO.FromEntity(c))
			.AsNoTracking()
            .ToListAsync();
    }

    public async Task<CompanyDTO?> GetCompanyByIdAsync(int id)
    {
        return await dbContext.Companies
            .Where(c => c.Id == id)
            .Select(c => CompanyDTO.FromEntity(c))
            .FirstOrDefaultAsync();
    }

    public async Task<CompanyDTO?> GetCompanyByIsinAsync(string isin)
    {
        return await dbContext.Companies
            .Where(c => c.Isin == isin)
            .Select(c => CompanyDTO.FromEntity(c))
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CompanyExistsAsync(int id)
    {
        return await dbContext.Companies
            .AnyAsync(c => c.Id == id);
    }

    public async Task<bool> DoesIsinExistAsync(string isin)
    {
        return await dbContext.Companies
            .AnyAsync(c => c.Isin == isin);
    }

    public async Task<int> CreateCompanyAsync(CreateCompanyDTO companyDTO)
    {
        var company = CreateCompanyDTO.ToEntity(companyDTO);
        dbContext.Companies.Add(company);
        await dbContext.SaveChangesAsync();
        return company.Id;
    }

    public async Task UpdateCompanyAsync(int id, UpdateCompanyDTO dto)
    {
        var company = await dbContext.Companies.FindAsync(id);

        company = UpdateCompanyDTO.ToEntity(dto);

        dbContext.Companies.Update(company);
        await dbContext.SaveChangesAsync();
    }
}

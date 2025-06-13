using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class CompanyRepository(ApplicationDbContext dbContext) : ICompanyRepository
{
    public async Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync()
    {
        return await dbContext.Companies
            .Select(c => CompanyDTO.FromEntity(c))
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

    public async Task<int> CreateCompanyAsync(Company company)
    {
        dbContext.Companies.Add(company);
        await dbContext.SaveChangesAsync();
        return company.Id;
    }

    public async Task UpdateCompanyAsync(int id, Company updatedCompany)
    {
        var company = await dbContext.Companies.FindAsync(id);

        company!.Name = updatedCompany.Name;
        company.Isin = updatedCompany.Isin;
        company.Ticker = updatedCompany.Ticker;
        company.Exchange = updatedCompany.Exchange;
        company.WebsiteUrl = updatedCompany.WebsiteUrl;

        dbContext.Companies.Update(company);
        await dbContext.SaveChangesAsync();
    }
}

namespace Application.Services;

using Application.DTOs;
using Domain.Entities;

public class CompanyService : ICompanyService
{
    public async Task<IEnumerable<CompanyDTO>> GetCompaniesAsync()
    {
        return await Task.FromResult(new List<CompanyDTO>
        {
            new CompanyDTO("Company A", "Exchange A", "Ticker A", new Isin("US1234567890"), "https://companya.com"),
            new CompanyDTO("Company B", "Exchange B", "Ticker B", new Isin("US0987654321"), "https://companyb.com")
        });
    }
}

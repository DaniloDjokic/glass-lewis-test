namespace Tests.Stubs;

using Application.DTOs;
using Application.Interfaces;

public class CompanyRepositoryStub : ICompanyRepository
{
    private readonly List<CompanyDTO> _companies = new()
    {
        new CompanyDTO(1,"Apple Inc.", "NASDAQ", "AAPL", "US0378331005", "https://www.apple.com"),
        new CompanyDTO(2,"Microsoft Corporation", "NASDAQ", "MSFT", "US5949181045", "https://www.microsoft.com"),
        new CompanyDTO(3,"Alphabet Inc.", "NASDAQ", "GOOGL", "US02079K3059", "https://www.google.com")
    };

    public IEnumerable<CompanyDTO> Companies => _companies;

    public Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync() => Task.FromResult<IEnumerable<CompanyDTO>>(_companies);

    public Task<CompanyDTO?> GetCompanyByIdAsync(int id) => Task.FromResult(_companies.FirstOrDefault(c => c.Id == id));

    public Task<CompanyDTO?> GetCompanyByIsinAsync(string isin) => Task.FromResult(_companies.FirstOrDefault(c => c.Isin == isin));

    public Task<bool> DoesIsinExistAsync(string isin) => Task.FromResult(_companies.Any(c => c.Isin == isin));

    public Task<bool> CompanyExistsAsync(int id) => Task.FromResult(_companies.Any(c => c.Id == id));

    public Task<int> CreateCompanyAsync(CreateCompanyDTO companyDto)
    {
        var newId = _companies.Max(c => c.Id) + 1;
        var newCompany = new CompanyDTO(newId, companyDto.Name, companyDto.Exchange, companyDto.Ticker, companyDto.Isin, companyDto.WebsiteUrl);

        _companies.Add(newCompany);
        return Task.FromResult(newId);
    }

    public Task UpdateCompanyAsync(int id, UpdateCompanyDTO updateCompanyDto)
    {
        var index = _companies.FindIndex(c => c.Id == id);

        if (index != -1)
        {
            var updatedCompany = _companies[index] with
            {
                Name = updateCompanyDto.Name,
                Exchange = updateCompanyDto.Exchange,
                Ticker = updateCompanyDto.Ticker,
                Isin = updateCompanyDto.Isin,
                WebsiteUrl = updateCompanyDto.WebsiteUrl
            };
            _companies[index] = updatedCompany;
        }

        return Task.CompletedTask;
    }
}

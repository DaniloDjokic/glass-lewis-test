using Domain.Entities;

namespace Application.DTOs;

public record UpdateCompanyDTO(
    string Name,
    string Exchange,
    string Ticker,
    string Isin,
    string? WebsiteUrl
)
{
    public void MapCompany(Company company)
    {
        company.Name = this.Name;
        company.Exchange = this.Exchange;
        company.Ticker = this.Ticker;
        company.Isin = this.Isin;
        company.WebsiteUrl = this.WebsiteUrl;
    }
}

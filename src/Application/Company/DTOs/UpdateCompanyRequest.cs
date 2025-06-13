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
    public Company MapFromDto(Company companyToUpdate)
    {
        companyToUpdate.Name = Name;
        companyToUpdate.Exchange = Exchange;
        companyToUpdate.Ticker = Ticker;
        companyToUpdate.Isin = Isin;
        companyToUpdate.WebsiteUrl = WebsiteUrl;

        return companyToUpdate;
    }
}

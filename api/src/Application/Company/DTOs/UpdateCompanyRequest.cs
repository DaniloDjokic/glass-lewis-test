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
    public static Company ToEntity(UpdateCompanyDTO dto) => new Company
    {
        Name = dto.Name,
        Exchange = dto.Exchange,
        Ticker = dto.Ticker,
        Isin = dto.Isin,
        WebsiteUrl = dto.WebsiteUrl
    };
}

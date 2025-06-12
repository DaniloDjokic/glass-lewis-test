using Domain.Entities;

namespace Application.DTOs;

public record CompanyDTO(
    string Name,
    string Exchange,
    string Ticker,
    Isin Isin,
    string? WebsiteUrl
)
{
    public static CompanyDTO FromEntity(Company company) => new(
        company.Name,
        company.Exchange,
        company.Ticker,
        company.Isin,
        company.WebsiteUrl
    );
}

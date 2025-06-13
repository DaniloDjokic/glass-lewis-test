using Domain.Entities;

namespace Application.DTOs;

public record CompanyDTO(
    int Id,
    string Name,
    string Exchange,
    string Ticker,
    string Isin,
    string? WebsiteUrl
)
{
    public static CompanyDTO FromEntity(Company company) => new(
            company.Id,
            company.Name,
            company.Exchange,
            company.Ticker,
            company.Isin,
            company.WebsiteUrl
    );

}

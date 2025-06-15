using Domain.Entities;

namespace Application.DTOs;

public record CreateCompanyDTO(
    string Name,
    string Exchange,
    string Ticker,
    string Isin,
    string? WebsiteUrl
)
{

    public static Company ToEntity(CreateCompanyDTO request) => new Company
    {
        Name = request.Name,
        Exchange = request.Exchange,
        Ticker = request.Ticker,
        Isin = request.Isin,
        WebsiteUrl = request.WebsiteUrl
    };
}

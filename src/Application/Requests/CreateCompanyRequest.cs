using Domain.Entities;

namespace Application.Requests;

public record CreateCompanyRequest(
    string Name,
    string Exchange,
    string Ticker,
    string Isin,
    string? WebsiteUrl
)
{

    public static Company ToEntity(CreateCompanyRequest request) => new Company
    {
        Name = request.Name,
        Exchange = request.Exchange,
        Ticker = request.Ticker,
        Isin = request.Isin,
        WebsiteUrl = request.WebsiteUrl
    };
}

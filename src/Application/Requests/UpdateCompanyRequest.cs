using Domain.Entities;

namespace Application.Requests;

public record UpdateCompanyRequest(
    string Name,
    string Exchange,
    string Ticker,
    string Isin,
    string? WebsiteUrl
)
{
    public static Company MapFromRequest(UpdateCompanyRequest request)
    {
        return new Company
        {
            Name = request.Name,
            Exchange = request.Exchange,
            Ticker = request.Ticker,
            Isin = request.Isin,
            WebsiteUrl = request.WebsiteUrl
        };
    }
}

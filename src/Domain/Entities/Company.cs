namespace Domain.Entities;

public class Company
{
    public string Name { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public required Isin Isin { get; set; }
    public string? WebsiteUrl { get; set; }
}

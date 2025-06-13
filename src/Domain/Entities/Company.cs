namespace Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Company
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Exchange { get; set; } = string.Empty;

    [Required]
    public string Ticker { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Isin { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? WebsiteUrl { get; set; }
}

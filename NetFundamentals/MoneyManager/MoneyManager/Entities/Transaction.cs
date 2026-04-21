using Microsoft.EntityFrameworkCore;

namespace MoneyManager.Entities;

public class Transaction
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    [Precision(16, 3)]
    public decimal Amount { get; set; }

    [Precision(7)]
    public DateTime Date { get; set; }

    public Guid AssetId { get; set; }
    public Asset Asset { get; set; }
    public string? Comment { get; set; }
}

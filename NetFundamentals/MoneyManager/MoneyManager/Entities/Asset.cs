using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Entities;

public class Asset
{
    public Guid Id { get; set; }

    [MaxLength(64)]
    public string Name { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public List<Transaction> Transactions { get; set; } = new();
}

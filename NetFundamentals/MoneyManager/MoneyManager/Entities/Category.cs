using MoneyManager.Constants;
using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Entities;

public class Category
{
    public Guid Id { get; set; }

    [MaxLength(64)]
    public string Name { get; set; }

    public int Type { get; set; }

    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }

    public List<Category> SubCategories { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
    public int Color { get; set; } = Colors.NileBlue;
}

using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Entities;

public class User
{
    public Guid Id { get; set; }

    [MaxLength(64)]
    public string Name { get; set; }

    [MaxLength(64)]
    public string Email { get; set; }

    [MaxLength(1024)]
    public string Hash { get; set; }

    [MaxLength(1024)]
    public string Salt { get; set; }

    public List<Asset> Assets { get; set; }
}

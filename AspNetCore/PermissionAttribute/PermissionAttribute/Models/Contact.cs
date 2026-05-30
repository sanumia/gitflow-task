using PermissionAttribute.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace PermissionAttribute.Models;

public class Contact
{
    public int ContactId { get; set; }

    [Required]
    public string OwnerID { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string State { get; set; }
    public string? Zip { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    public ContactStatus Status { get; set; }
}

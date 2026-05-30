using System.ComponentModel.DataAnnotations.Schema;

namespace CustomJsonFormatter.Models;

public class Profile
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<Article> Articles { get; set; }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace CustomJsonFormatter.Models;

public class Article
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public int? AuthorId { get; set; }
    public Profile? Author { get; set; }
}

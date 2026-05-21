namespace CustomJsonFormatter.DTOs;

public class ArticleDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; }
}

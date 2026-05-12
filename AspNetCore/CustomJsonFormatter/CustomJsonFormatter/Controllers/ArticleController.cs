using CustomJsonFormatter.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomJsonFormatter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticleController(JsonDbContext context) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Article>> GetArticles()
    {
        var articles = context.Articles.ToList();
        return Ok(articles);
    }

    [HttpGet("{id}")]
    public ActionResult GetArticleById(int id)
    {
        var article = context.Articles.Find(id);
        if (article is null)
            return NotFound();
        return Ok(article);
    }
}

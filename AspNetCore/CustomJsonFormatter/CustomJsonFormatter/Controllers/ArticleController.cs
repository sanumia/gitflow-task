using AutoMapper;
using CustomJsonFormatter.DTOs;
using CustomJsonFormatter.Helpers;
using CustomJsonFormatter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomJsonFormatter.Controllers;

[ApiController]
[Route("api/article")]
public class ArticleController : ControllerBase
{
    private readonly JsonDbContext _context;
    private readonly IMapper _mapper;

    public ArticleController(JsonDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<PagedResult<ArticleDto>> GetArticles(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Articles
            .Include(a => a.Author)
            .AsNoTracking();

        var totalCount = query.Count();   

        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();                   

        var itemDtos = _mapper.Map<List<ArticleDto>>(items);

        var result = new PagedResult<ArticleDto>
        {
            Items = itemDtos,
            PageIndex = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public ActionResult<ArticleDto> GetArticleById(int id)
    {
        var article = _context.Articles
            .Include(a => a.Author)
            .AsNoTracking()
            .FirstOrDefault(a => a.Id == id);

        return article is null
            ? NotFound()
            : Ok(_mapper.Map<ArticleDto>(article));
    }
}

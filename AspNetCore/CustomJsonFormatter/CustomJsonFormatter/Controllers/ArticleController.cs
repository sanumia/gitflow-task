using AutoMapper;
using CustomJsonFormatter.Consts;
using CustomJsonFormatter.DTOs;
using CustomJsonFormatter.Helpers;
using CustomJsonFormatter.Models;
using CustomJsonFormatter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomJsonFormatter.Controllers;

[ApiController]
[Route("api/article")]
public class ArticleController(IArticleService articleService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ArticleDto>>> GetArticles(
        [FromQuery] int pageNumber = PaginationConstants.DefaultPageNumber,
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize)
    {
        var result = await articleService.GetPagedArticlesAsync(pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDto>> GetArticleById(int id)
    {
        var article = await articleService.GetArticleByIdAsync(id);

        return article is null ? NotFound() : Ok(article);
    }
}

using AutoMapper;
using CustomJsonFormatter.Consts;
using CustomJsonFormatter.DTOs;
using CustomJsonFormatter.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CustomJsonFormatter.Services;

public class ArticleService(JsonDbContext context, IMapper mapper) : IArticleService
{
    public async Task<PagedResult<ArticleDto>> GetPagedArticlesAsync(int pageNumber, int pageSize)
    {
        pageNumber = pageNumber < 1 ? PaginationConstants.DefaultPageNumber : pageNumber;

        if (pageSize < PaginationConstants.MinPageSize)
            pageSize = PaginationConstants.DefaultPageSize;
        else if (pageSize > PaginationConstants.MaxPageSize)
            pageSize = PaginationConstants.MaxPageSize;

        var query = context.Articles
            .Include(a => a.Author)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var itemDtos = mapper.Map<List<ArticleDto>>(items);

        return new PagedResult<ArticleDto>
        {
            Items = itemDtos,
            PageIndex = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<ArticleDto?> GetArticleByIdAsync(int id)
    {
        var article = await context.Articles
            .Include(a => a.Author)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);

        return article is null ? null : mapper.Map<ArticleDto>(article);
    }
}

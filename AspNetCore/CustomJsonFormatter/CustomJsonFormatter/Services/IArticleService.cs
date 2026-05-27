using CustomJsonFormatter.DTOs;
using CustomJsonFormatter.Helpers;

namespace CustomJsonFormatter.Services;

public interface IArticleService
{
    Task<PagedResult<ArticleDto>> GetPagedArticlesAsync(int pageNumber, int pageSize);
    Task<ArticleDto?> GetArticleByIdAsync(int id);
}

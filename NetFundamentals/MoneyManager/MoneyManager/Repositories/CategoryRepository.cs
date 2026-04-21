using Microsoft.EntityFrameworkCore;
using MoneyManager.Entities;
using MoneyManager.Interfaces;
namespace MoneyManager.Repositories;

public class CategoryRepository : Repository<Category>
{
    public CategoryRepository(MoneyManagerContext context) : base(context) { }

    public async Task<Category?> GetCategoryWithHierarchyAsync(Guid id) =>
        await _dbSet
            .Include(c => c.Parent)
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id);
}

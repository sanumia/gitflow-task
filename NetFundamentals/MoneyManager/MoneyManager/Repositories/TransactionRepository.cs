using Microsoft.EntityFrameworkCore;
using MoneyManager.Entities;
using MoneyManager.Interfaces;

namespace MoneyManager.Repositories;

public class TransactionRepository : Repository<Transaction>
{
    public TransactionRepository(MoneyManagerContext context) : base(context) { }

    public async Task<Transaction?> GetTransactionWithDetailsAsync(Guid id) =>
        await _dbSet
            .Include(t => t.Category)
            .Include(t => t.Asset)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task DeleteUserTransactionsCurrentMonthAsync(Guid userId)
    {
        var now = DateTime.UtcNow;
        await _context.Transactions
            .Where(t => t.Asset.UserId == userId 
                    && t.Date.Year == now.Year 
                    && t.Date.Month == now.Month)
            .ExecuteDeleteAsync();
    }

    public async Task<List<TransactionListItem>> GetTransactionsByUserAsync(Guid userId) =>
        await _dbSet
            .Where(t => t.Asset.UserId == userId)
            .OrderByDescending(t => t.Date)
            .ThenBy(t => t.Asset.Name)
            .ThenBy(t => t.Category.Name)
            .Select(t => new TransactionListItem(
                t.Asset.Name,
                t.Category.Name,
                    t.Category.Parent != null 
                    ? t.Category.Parent.Name 
                    : null,
                t.Amount,
                t.Date,
                t.Comment))
            .ToListAsync();

    public async Task<List<MonthlyTotalsItem>> GetMonthlyTotalsAsync(
        Guid userId,
        DateTime startDate,
        DateTime endDate)
    {
        return await _dbSet
            .Where(t => t.Asset.UserId == userId && t.Date >= startDate && t.Date <= endDate)
            .GroupBy(t => new { t.Date.Year, t.Date.Month })
            .OrderBy(g => g.Key.Year)
            .ThenBy(g => g.Key.Month)
            .Select(g => new MonthlyTotalsItem(
                g.Key.Year,
                g.Key.Month,
                g.Where(t => t.Amount > 0).Sum(t => t.Amount),
                g.Where(t => t.Amount < 0).Sum(t => t.Amount)))
            .ToListAsync();
    }

    public async Task<List<CategoryTotalsItem>> GetCategoryTotalsAsync(Guid userId, int operationType)
    {
        var now = DateTime.UtcNow;

        var data = await _dbSet
            .Where(t => t.Asset.UserId == userId 
                    && t.Category.Type == operationType 
                    && t.Date.Year == now.Year 
                    && t.Date.Month == now.Month)
            .Select(t => new
            {
                t.Amount,
                CategoryName = t.Category.ParentId == null
                    ? t.Category.Name
                    : t.Category.Parent!.Name
            })
            .ToListAsync();

        return data
            .GroupBy(t => t.CategoryName)
            .Select(g => new CategoryTotalsItem(g.Key, g.Sum(t => t.Amount)))
            .OrderByDescending(x => x.Amount)
            .ThenBy(x => x.CategoryName)
            .ToList();
    }
}

using Microsoft.EntityFrameworkCore;
using MoneyManager.Entities;
using MoneyManager.Interfaces;

namespace MoneyManager.Repositories;

public class AssetRepository : Repository<Asset>
{
    public AssetRepository(MoneyManagerContext context) : base(context) 
    {
    }

    public async Task<List<AssetBalanceItem>> GetAssetsByUserAsync(Guid userId) =>
        await _dbSet
            .Where(a => a.UserId == userId)
            .OrderBy(a => a.Name)
            .Select(a => new AssetBalanceItem(
                a.Id,
                a.Name,
                a.Transactions.Sum(t => t.Amount)))
            .ToListAsync();
}

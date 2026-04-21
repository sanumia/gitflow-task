using Microsoft.EntityFrameworkCore;
using MoneyManager.Entities;
using MoneyManager.Interfaces;

namespace MoneyManager.Repositories;

public class UserRepository : Repository<User>
{
    public UserRepository(MoneyManagerContext context) : base(context) { }

    public async Task<User?> GetUserByEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<List<UserListItem>> GetUsersSortedByNameAsync() =>
        await _dbSet
            .OrderBy(u => u.Name)
            .Select(u => new UserListItem(u.Id, u.Name, u.Email))
            .ToListAsync();

    public async Task<UserListItem?> GetUserByEmailModelAsync(string email) =>
        await _dbSet
            .Where(u => u.Email == email)
            .Select(u => new UserListItem(u.Id, u.Name, u.Email))
            .FirstOrDefaultAsync();

    public async Task<UserBalanceItem?> GetUserBalanceAsync(Guid userId) =>
        await _dbSet
            .Where(u => u.Id == userId)
            .Select(u => new UserBalanceItem(
                u.Id,
                u.Email,
                u.Name,
                u.Assets.SelectMany(a => a.Transactions).Sum(t => t.Amount)))
            .FirstOrDefaultAsync();
}

using MoneyManager.Repositories;

namespace MoneyManager;

public static class UserBalanceHelper
{
    public static async Task PrintAllUserBalancesAsync(UserRepository userRepository)
    {
        var users = await userRepository.GetUsersSortedByNameAsync();
        Console.WriteLine("Users");
        foreach (var u in users)
            Console.WriteLine($"{u.Name}, {u.Email}");

        Console.WriteLine("\nBalances");
        foreach (var u in users)
        {
            var balance = await userRepository.GetUserBalanceAsync(u.Id);
            Console.WriteLine($"  {balance?.Name}: {balance?.Balance:F2}");
        }
    }

    public static async Task PrintUserAssetsAsync(
        UserRepository userRepository,
        AssetRepository assetRepository,
        Guid userId,
        string userName)
    {
        var assets = await assetRepository.GetAssetsByUserAsync(userId);
        Console.WriteLine($"\nAssets for {userName}");
        foreach (var a in assets)
            Console.WriteLine($"  {a.Name}: {a.Balance:F2}");
    }

    public static async Task PrintUserTransactionsAsync(
        TransactionRepository transactionRepository,
        Guid userId,
        string userName,
        int take = 5)
    {
        var txList = await transactionRepository.GetTransactionsByUserAsync(userId);
        Console.WriteLine($"\nTransactions for {userName} (first {take})");
        foreach (var t in txList.Take(take))
            Console.WriteLine($"  [{t.Date:yyyy-MM-dd}] {t.AssetName}, {t.CategoryParentName} ({t.CategoryName}): {t.Amount:F2}");
    }

    public static async Task PrintMonthlyTotalsAsync(
        TransactionRepository transactionRepository,
        Guid userId,
        string userName,
        int monthsBack = 6)
    {
        var monthly = await transactionRepository.GetMonthlyTotalsAsync(
            userId,
            DateTime.UtcNow.AddMonths(-monthsBack),
            DateTime.UtcNow);
        Console.WriteLine($"\nMonthly Totals for {userName} (last {monthsBack} months)");
        foreach (var m in monthly)
            Console.WriteLine($"  {m.Year}-{m.Month:D2}: Income={m.Income:F2}, Expenses={m.Expenses:F2}");
    }

    public static async Task PrintCategoryExpensesThisMonthAsync(
        TransactionRepository transactionRepository,
        Guid userId,
        string userName)
    {
        var catTotals = await transactionRepository.GetCategoryTotalsAsync(userId, operationType: 1);
        Console.WriteLine($"\nCategory Expenses this month for {userName}");
        foreach (var c in catTotals)
            Console.WriteLine($"  {c.CategoryName}: {c.Amount:F2}");
    }
}

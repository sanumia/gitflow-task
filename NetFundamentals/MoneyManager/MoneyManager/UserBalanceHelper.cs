using MoneyManager.Models;
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
        AssetRepository assetRepository,
        UserBaseInfo user)
    {
        var assets = await assetRepository.GetAssetsByUserAsync(user.Id);
        Console.WriteLine($"\nAssets for {user.Name}");
        foreach (var a in assets)
            Console.WriteLine($"  {a.Name}: {a.Balance:F2}");
    }

    public static async Task PrintUserTransactionsAsync(
        TransactionRepository transactionRepository,
        UserBaseInfo user,
        int takeAmount = 5)
    {
        var transactionsList = await transactionRepository.GetTransactionsByUserAsync(user.Id);
        Console.WriteLine($"\nTransactions for {user.Name} (first {takeAmount})");
        foreach (var t in transactionsList.Take(takeAmount))
            Console.WriteLine($"  [{t.Date:yyyy-MM-dd}] {t.AssetName}, {t.CategoryParentName} ({t.CategoryName}): {t.Amount:F2}");
    }

    public static async Task PrintMonthlyTotalsAsync(
        TransactionRepository transactionRepository,
        UserBaseInfo user,
        int monthsBack = 6)
    {
        var monthly = await transactionRepository.GetMonthlyTotalsAsync(
            user.Id,
            DateTime.UtcNow.AddMonths(-monthsBack),
            DateTime.UtcNow);
        Console.WriteLine($"\nMonthly Totals for {user.Name} (last {monthsBack} months)");
        foreach (var m in monthly)
            Console.WriteLine($"  {m.Year}-{m.Month:D2}: Income={m.Income:F2}, Expenses={m.Expenses:F2}");
    }

    public static async Task PrintCategoryExpensesThisMonthAsync(
        TransactionRepository transactionRepository,
        UserBaseInfo user)
    {
        var catTotals = await transactionRepository.GetCategoryTotalsAsync(user.Id, operationType: 1);
        Console.WriteLine($"\nCategory Expenses this month for {user.Name}");
        foreach (var c in catTotals)
            Console.WriteLine($"  {c.CategoryName}: {c.Amount:F2}");
    }
}

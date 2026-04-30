namespace MoneyManager;

public record UserListItem(Guid Id, string Name, string Email);

public record UserBalanceItem(Guid Id, string Email, string Name, decimal Balance);

public record AssetBalanceItem(Guid Id, string Name, decimal Balance);

public record TransactionListItem(
    string AssetName,
    string CategoryName,
    string? CategoryParentName,
    decimal Amount,
    DateTime Date,
    string? Comment
);

public record MonthlyTotalsItem(int Year, int Month, decimal Income, decimal Expenses);

public record CategoryTotalsItem(string CategoryName, decimal Amount);

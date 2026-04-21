using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MoneyManager;
using MoneyManager.Repositories;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
IConfigurationRoot configuration = builder.Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

var optionsBuilder = new DbContextOptionsBuilder<MoneyManagerContext>();
optionsBuilder.UseSqlServer(connectionString);

await using var context = new MoneyManagerContext(optionsBuilder.Options);

await context.Database.MigrateAsync();

var seeder = new DatabaseSeeder(context);
await seeder.SeedAsync();

var userRepository = new UserRepository(context);
var assetRepository = new AssetRepository(context);
var transactionRepository = new TransactionRepository(context);

await UserBalanceHelper.PrintAllUserBalancesAsync(userRepository);

var users = await userRepository.GetUsersSortedByNameAsync();
var firstUserId = users[0].Id;
var firstUserName = users[0].Name;

await UserBalanceHelper.PrintUserAssetsAsync(userRepository, assetRepository, firstUserId, firstUserName);

await UserBalanceHelper.PrintUserTransactionsAsync(transactionRepository, firstUserId, firstUserName, take: 5);

await UserBalanceHelper.PrintMonthlyTotalsAsync(transactionRepository, firstUserId, firstUserName, monthsBack: 6);

await UserBalanceHelper.PrintCategoryExpensesThisMonthAsync(transactionRepository, firstUserId, firstUserName);
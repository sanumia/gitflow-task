using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MoneyManager;
using MoneyManager.Models;
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
var firstUser = users[0];
var firstUserInfo = new UserBaseInfo { Id = firstUser.Id, Name = firstUser.Name };

await UserBalanceHelper.PrintUserAssetsAsync(assetRepository, firstUserInfo);
await UserBalanceHelper.PrintUserTransactionsAsync(transactionRepository, firstUserInfo, takeAmount: 5);
await UserBalanceHelper.PrintMonthlyTotalsAsync(transactionRepository, firstUserInfo, monthsBack: 6);
await UserBalanceHelper.PrintCategoryExpensesThisMonthAsync(transactionRepository, firstUserInfo);
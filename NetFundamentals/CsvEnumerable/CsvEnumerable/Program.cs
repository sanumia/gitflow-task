using CsvEnumerable;

string path = Path.Combine(AppContext.BaseDirectory, "Data", "user.csv");
var users = new CsvEnumerable<User>(path);
foreach (var user in users)
{
    Console.WriteLine(user.Name);
}
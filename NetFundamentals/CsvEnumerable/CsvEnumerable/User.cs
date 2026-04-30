using CsvHelper.Configuration.Attributes;

namespace CsvEnumerable;

public class User
{
    [Name("user_id")]
    public int Id {  get; set; }

    [Name("name")]
    public string Name { get; set; }

    [Name("email")]
    public string Email { get; set; }

    [Name("created_at")]
    public DateTime DateOfCreation { get; set; }
}

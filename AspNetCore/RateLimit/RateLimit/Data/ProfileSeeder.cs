using RateLimit.Models;
using System.Text.Json;

namespace RateLimit.Data;

public class ProfileSeeder(IWebHostEnvironment env)
{
    public void Seed()
    {
        var filePath = Path.Combine(env.ContentRootPath, "Data", "profiles.json");

        var profiles = new List<Profile>
        {
            new Profile
            {
                Id = Guid.NewGuid(),
                FirstName = "Anna",
                LastName = "Smith",
                Birthday = new DateTime(1990, 4, 15)
            },
            new Profile
            {
                Id = Guid.NewGuid(),
                FirstName = "Max",
                LastName = "Brown",
                Birthday = new DateTime(1985, 11, 22)
            },
            new Profile
            {
                Id = Guid.NewGuid(),
                FirstName = "Mary",
                LastName = "Miller",
                Birthday = new DateTime(1995, 7, 3)
            }
        };

        var json = JsonSerializer.Serialize(profiles, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, json);
    }
}

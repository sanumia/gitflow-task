using CustomJsonFormatter.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomJsonFormatter;

public class DatabaseSeeder(JsonDbContext context)
{
    public async Task SeedAsync()
    {
        if (await context.Profiles.AnyAsync())
            return;

        await using var transaction = await context.Database.BeginTransactionAsync(); 
        try
        {
            var profiles = GetProfiles();
            await context.Profiles.AddRangeAsync(profiles);
            await context.SaveChangesAsync();

            var articles = GetArticles(profiles);
            await context.Articles.AddRangeAsync(articles);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private List<Profile> GetProfiles()
    {
        return new List<Profile>
        {
            new() {Name = "John Doe", Email = "john@gmail.com"},
            new() {Name = "Jane Smith", Email = "jane@gmail.com"},
            new() {Name = "Emma Stone", Email = "emma@gmail.com"},
            new() {Name = "Emily Clark", Email = "emily@gmail.com"}
        };
    }

    private List<Article> GetArticles(List<Profile> profiles)
    {
        var john = profiles.First(p => p.Name == "John Doe");
        var jane = profiles.First(p => p.Name == "Jane Smith");
        var emily = profiles.First(p => p.Name == "Emily Clark");

        return new List<Article>
        {
            new() {Title = "Article1", Description = "Article1 by John Doe", AuthorId = john.Id},
            new() {Title = "Article2", Description = "Article2 by Jane Smith", AuthorId = jane.Id},
            new() {Title = "Article3", Description = "Artcile3 by Emily Clark", AuthorId = emily.Id},
            new() {Title = "Article4", Description = "Article4 by Emily Clark", AuthorId = emily.Id},
        };
    }
}

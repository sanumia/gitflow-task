using CustomJsonFormatter.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomJsonFormatter;

public class JsonDbContext : DbContext
{
    public JsonDbContext(DbContextOptions<JsonDbContext> options) : base(options) 
    {
    }

    public DbSet<Article> Articles { get; set; }
    public DbSet<Profile> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Article>()   
            .HasOne(a => a.Author)
            .WithMany(p => p.Articles)
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Serialization;

public class SerializationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = configuration
            .GetConnectionString("DefaultConnection");

        options.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id)
                  .ValueGeneratedNever();

            entity.Property(u => u.Name)
                  .IsRequired()
                  .HasMaxLength(256);

            entity.Property(u => u.Email)
                  .IsRequired()
                  .HasMaxLength(256);
        });
    }
}

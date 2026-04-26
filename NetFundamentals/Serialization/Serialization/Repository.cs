using Microsoft.EntityFrameworkCore;

namespace Serialization;

public class Repository<T> where T : class
{
    private readonly SerializationDbContext _context;
    private readonly DbSet<T> _set;

    public Repository(SerializationDbContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public IEnumerable<T> GetAll() => [.. _set.AsNoTracking()];

    public void AddRange(IEnumerable<T> entities)
    {
        _set.AddRange(entities);
        _context.SaveChanges();
    }
}

using TimeTracking.Models;

namespace TimeTracking.Services;

public class ProfileService : IProfileService
{
    private readonly List<Profile> _profiles = new();
    private int counterForId = 1;

    public ProfileService()
    {
        Add(new Profile { Name = "John Doe", Email = "john@example.com" });
        Add(new Profile { Name = "Jane Smith", Email = "jane@example.com" });
    }

    public Profile? GetById(int id)
    {
        return _profiles.FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<Profile> GetAll()
    {
        return _profiles.OrderBy(p => p.Id);
    }

    public void Add(Profile profile)
    {
        profile.Id = counterForId++;
        _profiles.Add(profile);
    }

    public void Update(Profile profile)
    {
        var existing = GetById(profile.Id);
        if (existing != null)
        {
            existing.Name = profile.Name;
            existing.Email = profile.Email;
        }
    }

    public void Delete(int id)
    {
        var profile = GetById(id);
        if (profile != null)
        {
            _profiles.Remove(profile);
        }
    }
}

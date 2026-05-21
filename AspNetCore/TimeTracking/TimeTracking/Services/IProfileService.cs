using TimeTracking.Models;

namespace TimeTracking.Services;

public interface IProfileService
{
    Profile GetById(int id);
    IEnumerable<Profile> GetAll();
    void Add(Profile profile);
    void Update(Profile profile);
    void Delete(int id);
}

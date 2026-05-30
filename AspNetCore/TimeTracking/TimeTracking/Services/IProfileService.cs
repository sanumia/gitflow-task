using TimeTracking.Models;

namespace TimeTracking.Services;

public interface IProfileService
{
    Profile GetById(int id);
    IEnumerable<Profile> GetAll();
    void Add(Profile profile);
    bool Update(Profile profile);
    bool Delete(int id);
}

using RateLimit.Models;

namespace RateLimit.Services;

public interface IProfileService
{
    Task<PagedResultModel<Profile>> GetProfilesAsync(ProfileQueryModel query);
}

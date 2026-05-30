namespace PermissionAttribute.Services;

public interface ICurrentUserService
{
    Task<(string userId, string? role)> GetCurrentUserInfoAsync();
}

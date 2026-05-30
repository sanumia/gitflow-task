using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace PermissionAttribute.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager) : ICurrentUserService
{
    public async Task<(string userId, string? role)> GetCurrentUserInfoAsync()
    {
        var context = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("No HTTP context available.");

        var user = context.User;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User ID claim not found.");

        var identityUser = await userManager.GetUserAsync(user)
            ?? throw new InvalidOperationException("User not found.");

        var roles = await userManager.GetRolesAsync(identityUser);

        return (userId, roles.FirstOrDefault());
    }
}

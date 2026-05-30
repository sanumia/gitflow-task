using Microsoft.AspNetCore.Authorization;

namespace PermissionAttribute.Handlers;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private const string Permission = "Permission";
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var hasClaim = context.User.Claims.Any(c =>
            c.Type == Permission &&
            c.Value == requirement.Permission);

        if (hasClaim)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
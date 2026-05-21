using Microsoft.AspNetCore.Authorization;
using PermissionAttribute.Models.Enums;

namespace PermissionAttribute.Attributes;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permissions permission)
    {
        Policy = permission.ToString();
    }
}


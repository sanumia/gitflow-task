using Microsoft.AspNetCore.Identity;
using PermissionAttribute.Models;
using PermissionAttribute.Models.Enums;
using System.Security.Claims;

namespace PermissionAttribute;

public class SeedData
{
    public static async Task Initialize(IServiceProvider sp, string password)
    {
        var userManager = sp.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
        var context = sp.GetRequiredService<PermissionDbContext>();

        await context.Database.EnsureCreatedAsync();

        await EnsureRole(roleManager, Roles.Admin.ToString());
        await EnsureRole(roleManager, Roles.Manager.ToString());

        await AssignPermissionsToRole(roleManager, Roles.Admin, GetAllPermissions());
        await AssignPermissionsToRole(roleManager, Roles.Manager, GetManagerPermissions());

        var admin = await EnsureUser(userManager, password, "admin@contoso.com");
        var manager = await EnsureUser(userManager, password, "manager@contoso.com");

        await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
        await userManager.AddToRoleAsync(manager, Roles.Manager.ToString());

        SeedDB(context, admin.Id);
    }

    private static List<Permissions> GetAllPermissions()
    {
        return Enum.GetValues<Permissions>().ToList();
    }

    private static List<Permissions> GetManagerPermissions()
    {
        return new List<Permissions>
        {
            Permissions.GetProfileById,
            Permissions.GetProfiles,
            Permissions.AddProfile,
            Permissions.UpdateProfile
        };
    }

    private static async Task AssignPermissionsToRole(
        RoleManager<IdentityRole> roleManager,
        Roles role,
        List<Permissions> permissions)
    {
        var identityRole = await roleManager.FindByNameAsync(role.ToString());
        if (identityRole == null)
            throw new Exception($"Role '{role}' not found");

        var existingClaims = await roleManager.GetClaimsAsync(identityRole);

        foreach (var perm in permissions)
        {
            var claimValue = perm.ToString();
            if (!existingClaims.Any(c => c.Type == "Permission" && c.Value == claimValue))
            {
                await roleManager.AddClaimAsync(identityRole, new Claim("Permission", claimValue));
            }
        }
    }

    private static async Task EnsureRole(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    private static async Task<IdentityUser> EnsureUser(
        UserManager<IdentityUser> userManager,
        string password,
        string email)
    {
        var user = await userManager.FindByNameAsync(email);
        if (user != null)
            return user;

        user = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
            return user;
        if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
        {
            user = await userManager.FindByNameAsync(email);
            if (user != null)
                return user;
        }
        var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
        throw new Exception($"User creation failed for {email}: {errors}");
    }

    public static void SeedDB(PermissionDbContext context, string adminID)
    {
        if (context.Contacts.Any()) return;

        context.Contacts.AddRange(
            new Contact
            {
                Name = "Debra Garcia",
                Address = "1234 Main St",
                City = "Redmond",
                State = "WA",
                Zip = "10999",
                Email = "debra@example.com",
                Status = ContactStatus.Approved,
                OwnerID = adminID
            },
            new Contact
            {
                Name = "Thorsten Weinrich",
                Address = "5678 1st Ave W",
                City = "Redmond",
                State = "WA",
                Zip = "10999",
                Email = "thorsten@example.com",
                Status = ContactStatus.Submitted,
                OwnerID = adminID
            },
            new Contact
            {
                Name = "Yuhong Li",
                Address = "9012 State st",
                City = "Redmond",
                State = "WA",
                Zip = "10999",
                Email = "yuhong@example.com",
                Status = ContactStatus.Rejected,
                OwnerID = adminID
            },
            new Contact
            {
                Name = "Jon Orton",
                Address = "3456 Maple St",
                City = "Redmond",
                State = "WA",
                Zip = "10999",
                Email = "jon@example.com",
                Status = ContactStatus.Submitted,
                OwnerID = adminID
            },
            new Contact
            {
                Name = "Diliana Alexieva-Bosseva",
                Address = "7890 2nd Ave E",
                City = "Redmond",
                State = "WA",
                Zip = "10999",
                Email = "diliana@example.com",
                OwnerID = adminID
            }
        );
        context.SaveChanges();
    }
}
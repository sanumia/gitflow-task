using Microsoft.AspNetCore.Identity;
using PermissionAttribute.Models;
using PermissionAttribute.Models.Enums;
using System.Security.Claims;

namespace PermissionAttribute;

public class SeedData
{
    private const string DuplicateUserNameErrorCode = "DuplicateUserName";
    private const string PermissionClaimType = "Permission";
    private const string AdminEmail = "admin@contoso.com";
    private const string ManagerEmail = "manager@contoso.com";

    public static async Task Initialize(IServiceProvider serviceProvider, string adminPassword)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var dbContext = serviceProvider.GetRequiredService<PermissionDbContext>();

        await dbContext.Database.EnsureCreatedAsync();

        await EnsureRoleAsync(roleManager, Roles.Admin.ToString());
        await EnsureRoleAsync(roleManager, Roles.Manager.ToString());

        await AssignPermissionsToRoleAsync(roleManager, Roles.Admin, GetAllPermissions());
        await AssignPermissionsToRoleAsync(roleManager, Roles.Manager, GetManagerPermissions());

        var adminUser = await EnsureUserAsync(userManager, adminPassword, AdminEmail);
        var managerUser = await EnsureUserAsync(userManager, adminPassword, ManagerEmail);

        await userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
        await userManager.AddToRoleAsync(managerUser, Roles.Manager.ToString());

        await SeedContactsAsync(dbContext, adminUser.Id);
    }

    private static List<Permissions> GetAllPermissions()
    {
        return Enum.GetValues<Permissions>().ToList();
    }

    private static List<Permissions> GetManagerPermissions()
    {
        return new List<Permissions>
        {
            Permissions.GetContactById,
            Permissions.GetContacts,
            Permissions.AddContact,
            Permissions.UpdateContact
        };
    }

    private static async Task AssignPermissionsToRoleAsync(
        RoleManager<IdentityRole> roleManager,
        Roles role,
        List<Permissions> permissions)
    {
        var roleName = role.ToString();
        var identityRole = await roleManager.FindByNameAsync(roleName);

        if (identityRole is null)
        {
            throw new InvalidOperationException(
                $"Role '{roleName}' not found. Ensure the role is created before assigning permissions.");
        }

        var existingClaims = await roleManager.GetClaimsAsync(identityRole);

        foreach (var permission in permissions)
        {
            var claimValue = permission.ToString();
            bool claimAlreadyExists = existingClaims.Any(c =>
                c.Type == PermissionClaimType && c.Value == claimValue);

            if (!claimAlreadyExists)
            {
                await roleManager.AddClaimAsync(identityRole, new Claim(PermissionClaimType, claimValue));
            }
        }
    }

    private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
    {
        bool roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    private static async Task<IdentityUser> EnsureUserAsync(
        UserManager<IdentityUser> userManager,
        string password,
        string email)
    {
        var user = await userManager.FindByNameAsync(email);
        if (user is not null)
        {
            return user;
        }

        user = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var creationResult = await userManager.CreateAsync(user, password);

        if (creationResult.Succeeded)
        {
            return user;
        }

        bool isDuplicate = creationResult.Errors.Any(e => e.Code == DuplicateUserNameErrorCode);
        if (isDuplicate)
        {
            user = await userManager.FindByNameAsync(email);
            if (user is not null)
            {
                return user;
            }
        }

        var errors = string.Join("; ", creationResult.Errors.Select(e => $"{e.Code}: {e.Description}"));
        throw new InvalidOperationException($"User creation failed for {email}: {errors}");
    }

    private static async Task SeedContactsAsync(PermissionDbContext dbContext, string ownerId)
    {
        if (dbContext.Contacts.Any())
        {
            return;
        }

        var contacts = GetSampleContacts(ownerId);
        await dbContext.Contacts.AddRangeAsync(contacts);
        await dbContext.SaveChangesAsync();
    }

    private static IEnumerable<Contact> GetSampleContacts(string ownerId)
    {
        return new List<Contact>
        {
            new Contact
            {
                Name = "Debra Garcia",
                Address = "1234 Main St",
                City = "Redmond",
                State = "WA",
                Zip = "10999",
                Email = "debra@example.com",
                Status = ContactStatus.Approved,
                OwnerID = ownerId
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
                OwnerID = ownerId
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
                OwnerID = ownerId
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
                OwnerID = ownerId
            },
            new Contact
            {
                Name = "Diliana Alexieva-Bosseva",
                Address = "7890 2nd Ave E",
                City = "Redmond",
                State = "WA",
                Zip = "10999",
                Email = "diliana@example.com",
                OwnerID = ownerId
            }
        };
    }
}
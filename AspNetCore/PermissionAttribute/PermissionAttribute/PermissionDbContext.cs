using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PermissionAttribute.Models;

namespace PermissionAttribute;

public class PermissionDbContext(DbContextOptions<PermissionDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Contact>  Contacts { get; set; }

}

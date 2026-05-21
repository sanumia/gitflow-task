using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PermissionAttribute;
using PermissionAttribute.Handlers;
using PermissionAttribute.Models.Enums;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PermissionDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PermissionDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToAreaPage(
        "Identity",
        "/Account/Login");

    options.Conventions.AllowAnonymousToAreaPage(
        "Identity",
        "/Account/Register");

    options.Conventions.AllowAnonymousToAreaPage(
        "Identity",
        "/Account/Logout");

    options.Conventions.AllowAnonymousToAreaPage(
        "Identity",
        "/Account/AccessDenied");
});
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddAuthorization(options =>
{
    foreach (var permission in Enum.GetValues<Permissions>())
    {
        options.AddPolicy(permission.ToString(), policy =>
        {
            policy.Requirements.Add(
                new PermissionRequirement(permission.ToString()));
        });
    }
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await SeedData.Initialize(
        services,
        "Pa$$w0rd");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
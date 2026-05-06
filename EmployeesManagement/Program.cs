using EmployeesManagement.Data;
using EmployeesManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EmployeesManagement.Profiles;
using EmployeesManagement.Services;

var builder = WebApplication.CreateBuilder(args);

//
// =========================
// DATABASE CONFIG
// =========================
//
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine("FINAL CONNECTION STRING:");
Console.WriteLine(connectionString);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(9, 7, 0)) // stable for Docker MySQL
    )
);

//
// =========================
// IDENTITY CONFIG
// =========================
//
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

//
// =========================
// MVC + RAZOR
// =========================
//
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//
// =========================
// AUTH
// =========================
//
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

//
// =========================
// AUTO MAPPER
// =========================
//
builder.Services.AddAutoMapper(typeof(AutomapperProfiles));

//
// =========================
// CUSTOM SERVICES
// =========================
//
builder.Services.AddTransient<IExtensionService, ExtensionService>();

var app = builder.Build();

//
// =========================
// MIDDLEWARE
// =========================
//
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//
// =========================
// ROUTES
// =========================
//
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

//
// =========================
// SEED ROLES (FIXED)
// =========================
//
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!roleManager.RoleExistsAsync(role).Result)
        {
            roleManager.CreateAsync(new IdentityRole(role)).Wait();
        }
    }
}

app.Run();
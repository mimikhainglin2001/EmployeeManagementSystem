using EmployeesManagement.Data;
using EmployeesManagement.Models;
using EmployeesManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UsersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Include(x=>x.Role).ToListAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            try
            {
                // Always reload roles
                ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", model.RoleId);

                // 🔍 DEBUG ModelState
                if (!ModelState.IsValid)
                {
                    foreach (var item in ModelState)
                    {
                        foreach (var error in item.Value.Errors)
                        {
                            Console.WriteLine($"MODEL ERROR ({item.Key}): {error.ErrorMessage}");
                        }
                    }

                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    NationalId = model.NationalId,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    CreatedById = _userManager.GetUserId(User)
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                // 🔴 CRITICAL DEBUG POINT
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine("IDENTITY ERROR: " + error.Description);
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }

                // ✅ Assign Role
                var role = await _roleManager.FindByIdAsync(model.RoleId);

                if (role == null)
                {
                    ModelState.AddModelError("", "Role not found!");
                    return View(model);
                }

                var roleResult = await _userManager.AddToRoleAsync(user, role.Name);

                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        Console.WriteLine("ROLE ERROR: " + error.Description);
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
                TempData["Message"] = "User create successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "User creation failed" + ex.Message;
                return View(model);

            }
        }
    }
}
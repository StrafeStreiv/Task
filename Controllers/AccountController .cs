using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSys.Data;
using TaskManagementSys.Models;
using TaskManagementSys.ViewModels;

namespace TaskManagementSys.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        // === LOGIN ===
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
                return Redirect(returnUrl ?? "/");

            ModelState.AddModelError("", "Неверный логин или пароль");
            return View(model);
        }

        // === REGISTER ===
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var defaultTeam = await _context.Teams.FirstOrDefaultAsync(t => t.Name == "Default");
                if (defaultTeam != null)
                {
                    user.TeamId = defaultTeam.Id;
                    await _userManager.UpdateAsync(user);
                }

                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Tasks");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.Users
                .Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            var teamUsers = await _context.Users
                .Where(u => u.TeamId == user.TeamId && u.Id != user.Id)
                .Select(u => u.Email)
                .ToListAsync();

            var model = new ProfileViewModel
            {
                Email = user.Email,
                FullName = user.FullName,
                Theme = user.Theme,
                TeamName = user.Team?.Name ?? "Без команды",
                TeamMembers = teamUsers
            };

            return View(model);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                user.FullName = model.FullName;
                user.Theme = model.Theme;
                await _userManager.UpdateAsync(user);

                ViewBag.StatusMessage = "✅ Профиль обновлён";
                model.Email = user.Email;
                model.TeamName = (await _context.Teams.FindAsync(user.TeamId))?.Name ?? "Без команды";
            }

            return View(model);
        }



        // === LOGOUT ===
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

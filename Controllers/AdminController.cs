using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagementSys.Data;
using TaskManagementSys.Models;

namespace TaskManagementSys.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: /Admin
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Include(u => u.Team).ToListAsync();

            var userList = new List<AdminUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new AdminUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = string.Join(", ", roles),
                    TeamName = user.Team?.Name ?? "—",
                    TeamId = user.TeamId
                });
            }

            ViewBag.Teams = new SelectList(await _context.Teams.ToListAsync(), "Id", "Name");

            return View(userList);
        }

        // POST: /Admin/ChangeTeam
        [HttpPost]
        public async Task<IActionResult> ChangeTeam(string id, int teamId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            user.TeamId = teamId;
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }
    }
}

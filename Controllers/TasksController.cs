using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagementSys.Data;
using TaskManagementSys.Models;

namespace TaskManagementSys.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<TaskItem> tasks;

            if (User.IsInRole("Admin"))
            {
                tasks = await _context.Tasks
                    .Include(t => t.User)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
            }
            else
            {
                tasks = await _context.Tasks
                    .Where(t => t.UserId == currentUserId)
                    .Include(t => t.User)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
            }

            return View(tasks);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.Tasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null) return NotFound();

            if (!User.IsInRole("Admin") && task.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.Users = new SelectList(_context.Users, "Id", "Email");
            }

            return View();
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem task)
        {
            if (!User.IsInRole("Admin"))
            {
                task.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (User.IsInRole("Admin"))
            {
                ViewBag.Users = new SelectList(_context.Users, "Id", "Email", task.UserId);
            }

            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            if (!User.IsInRole("Admin") && task.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            if (User.IsInRole("Admin"))
                ViewBag.Users = new SelectList(_context.Users, "Id", "Email", task.UserId);

            return View(task);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItem task)
        {
            if (id != task.Id) return NotFound();

            var existing = await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
            if (existing == null) return NotFound();

            if (!User.IsInRole("Admin") && existing.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            if (!User.IsInRole("Admin"))
                task.UserId = existing.UserId;

            if (ModelState.IsValid)
            {
                _context.Update(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (User.IsInRole("Admin"))
                ViewBag.Users = new SelectList(_context.Users, "Id", "Email", task.UserId);

            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.Tasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null) return NotFound();

            if (!User.IsInRole("Admin") && task.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            if (!User.IsInRole("Admin") && task.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Kanban(string userId, TaskPriority? priority)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IQueryable<TaskItem> query = _context.Tasks.Include(t => t.User);

            if (!User.IsInRole("Admin"))
                query = query.Where(t => t.UserId == currentUserId);

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(t => t.UserId == userId);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            ViewBag.Users = new SelectList(_context.Users, "Id", "Email", userId);
            ViewBag.SelectedPriority = priority;

            var tasks = await query.ToListAsync();

            var grouped = tasks
                .GroupBy(t => t.Status)
                .ToDictionary(g => g.Key, g => g.ToList());

            return View(grouped);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus([FromBody] StatusUpdateModel model)
        {
            if (!Enum.TryParse<Models.TaskStatus>(model.Status, out var newStatus))
                return BadRequest();

            var task = await _context.Tasks.FindAsync(model.Id);
            if (task == null) return NotFound();

            if (!User.IsInRole("Admin") && task.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            task.Status = newStatus;
            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> EditModal(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            if (!User.IsInRole("Admin") && task.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            return PartialView("_EditModal", task);
        }



    }
}

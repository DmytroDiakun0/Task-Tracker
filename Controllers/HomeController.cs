using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;

namespace TaskTracker.Controllers
{
    public class HomeController : Controller
    {
        TaskTrackerDbContext _context;

        public HomeController(TaskTrackerDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string[] statuses, string sortBy, string sortingType)
        {
            if (statuses == null || statuses.Length == 0)
            {
                statuses = ["Todo", "InProgress", "Done"];
            }
            sortBy = sortBy ?? "Id";
            sortingType = sortingType ?? "Asc";

            ViewData["ActiveStatuses"] = statuses;
            ViewData["SortBy"] = sortBy;
            ViewData["SortingType"] = sortingType;

            var enumStatuses = statuses.Select(s => Enum.Parse<Models.Task.TaskStatus>(s)).ToList();
            var tasks = from t in _context.Tasks
                        where enumStatuses.Contains(t.Status)
                        select t;

            switch (sortBy)
            {
                case "Id":
                    tasks = sortingType == "Desc" ? tasks.OrderByDescending(t => t.Id) : tasks.OrderBy(t => t.Id);
                    break;
                case "Description":
                    tasks = sortingType == "Desc" ? tasks.OrderByDescending(t => t.Description) : tasks.OrderBy(t => t.Description);
                    break;
                case "Status":
                    tasks = sortingType == "Desc" ? tasks.OrderByDescending(t => t.Status) : tasks.OrderBy(t => t.Status);
                    break;
                case "Created At":
                    tasks = sortingType == "Desc" ? tasks.OrderByDescending(t => t.CreatedAt) : tasks.OrderBy(t => t.CreatedAt);
                    break;
                case "Updated At":
                    tasks = sortingType == "Desc" ? tasks.OrderByDescending(t => t.UpdatedAt) : tasks.OrderBy(t => t.UpdatedAt);
                    break;
            }

            return View(tasks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Description, Status, CreatedAt, UpdatedAt")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                task.CreatedAt = DateTime.Now;
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, [Bind("Id, Description, Status, CreatedAt, UpdatedAt")] Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                task.UpdatedAt = DateTime.Now;
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}

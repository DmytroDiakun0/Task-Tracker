using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;

namespace TaskTracker.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new TaskTrackerDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<TaskTrackerDbContext>>()))
        {
            if (context.Tasks.Any())
            {
                return;
            }

            context.Tasks.AddRange(
                new Models.Task
                {
                    Description = "Complete project documentation",
                    Status = Task.TaskStatus.Todo,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Models.Task
                {
                    Description = "Develop unit tests",
                    Status = Task.TaskStatus.InProgress,
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Models.Task
                {
                    Description = "Fix bug #342",
                    Status = Task.TaskStatus.Done,
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Models.Task
                {
                    Description = "Implement new UI feature",
                    Status = Task.TaskStatus.InProgress,
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow
                },
                new Models.Task
                {
                    Description = "Review codebase for refactoring",
                    Status = Task.TaskStatus.Todo,
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow
                },
                new Models.Task
                {
                    Description = "Prepare presentation for team meeting",
                    Status = Task.TaskStatus.Done,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow
                }
            );

            context.SaveChanges();
        }
    }
}

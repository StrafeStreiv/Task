using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSys.Models;

namespace TaskManagementSys.Data
{
    public static class RoleInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //  1. Добавляем роли
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 2. Создаем команды
            string[] teamNames = { "Default", "Back", "Front", "Design" };
            foreach (var name in teamNames)
            {
                if (!await context.Teams.AnyAsync(t => t.Name == name))
                    context.Teams.Add(new Team { Name = name });
            }

            await context.SaveChangesAsync();

            //  3. Привязываем пользователей и задачи к Default, если TeamId == null
            var defaultTeam = await context.Teams.FirstOrDefaultAsync(t => t.Name == "Default");
            if (defaultTeam != null)
            {
                var users = await context.Users.Where(u => u.TeamId == null).ToListAsync();
                foreach (var user in users)
                {
                    user.TeamId = defaultTeam.Id;
                }

                var tasks = await context.Tasks.Where(t => t.TeamId == null).ToListAsync();
                foreach (var task in tasks)
                {
                    task.TeamId = defaultTeam.Id;
                }

                await context.SaveChangesAsync();
            }

            //  4. Создаем администратора
            var adminEmail = "admin@site.com";
            var adminPassword = "Admin123!";

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    TeamId = defaultTeam?.Id
                };

                var result = await userManager.CreateAsync(user, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}

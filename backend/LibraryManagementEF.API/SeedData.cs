using LibraryManagementEF.Core.Entities;
using LibraryManagementEF.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementEF.API.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            context.Database.Migrate();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new Role { Name = "Admin" });
            }
            if (!await roleManager.RoleExistsAsync("Member"))
            {
                await roleManager.CreateAsync(new Role { Name = "Member" });
            }

            var adminEmail = "admin@library.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User"
                };

                var createAdmin = await userManager.CreateAsync(newAdmin, "Admin@123");
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
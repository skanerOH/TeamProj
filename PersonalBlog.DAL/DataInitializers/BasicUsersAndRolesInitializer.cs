using Microsoft.AspNetCore.Identity;
using PersonalBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.DAL.DataInitializers
{
    public class BasicUsersAndRolesInitializer
    {
        public static async Task InitializeAsync(UserManager<UserWithIdentity> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "Abc_123";
            string adminFullName = "Built-in admin";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                UserWithIdentity admin = new UserWithIdentity { Email = adminEmail, UserName = adminEmail, FullName = adminFullName };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }

}

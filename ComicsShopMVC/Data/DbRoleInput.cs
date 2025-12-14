using Microsoft.AspNetCore.Identity;

namespace ComicsShopMVC.Data
{
    public static class DbRoleInput
    {
        public static async Task InputRolesAdmin(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<IdentityUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            var adminEmail = "admin@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var testUserEmail = "testuser@gmail.com";
            var testUserPassword = "User123!";
            var existingUser = await userManager.FindByEmailAsync(testUserEmail);
            if (existingUser == null)
            {
                var newUser = new IdentityUser
                {
                    UserName = testUserEmail,
                    Email = testUserEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newUser, testUserPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "User");
                }
            }
        }
    }
}
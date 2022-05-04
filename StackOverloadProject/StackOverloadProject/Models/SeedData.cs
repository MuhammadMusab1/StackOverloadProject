using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackOverloadProject.Data;

namespace StackOverloadProject.Models
{
    public class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!context.Roles.Any())
            {
                //create new roles

                List<string> newRoles = new List<string>()
                {
                    "Admin",
                    "Instructor",
                    "Manager"
                };
                foreach (string role in newRoles)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            if (!context.Users.Any())
            {
                //create new users
                var passwordHasher = new PasswordHasher<ApplicationUser>();

                //user1
                ApplicationUser firstUser = new ApplicationUser()
                {
                    Email = "musab03@gmail.com",
                    NormalizedEmail = "MUSAB03@GMAIL.COM",
                    UserName = "musab03@gmail.com",
                    NormalizedUserName = "MUSAB03@GMAIL.COM",
                    EmailConfirmed = true,
                };
                var firstUserHashedPassword = passwordHasher.HashPassword(firstUser, "Yaqoob@12");
                firstUser.PasswordHash = firstUserHashedPassword;
                await userManager.CreateAsync(firstUser);

                //user2
                ApplicationUser secondUser = new ApplicationUser()
                {
                    Email = "eisha03@gmail.com",
                    NormalizedEmail = "EISHA03@GMAIL.COM",
                    UserName = "eisha03@gmail.com",
                    NormalizedUserName = "EISHA03@GMAIL.COM",
                    EmailConfirmed = true,
                };
                var secondUserHashedPassword = passwordHasher.HashPassword(secondUser, "Yaqoob@12");
                secondUser.PasswordHash = secondUserHashedPassword;

                await userManager.CreateAsync(secondUser);
                await userManager.AddToRoleAsync(secondUser, "Admin");

                //user3
                ApplicationUser thridUser = new ApplicationUser()
                {
                    Email = "ahmed03@gmail.com",
                    NormalizedEmail = "AHMED03@GMAIL.COM",
                    UserName = "ahmed03@gmail.com",
                    NormalizedUserName = "AHMED03@GMAIL.COM",
                    EmailConfirmed = true,
                };
                var thridUserHashedPassword = passwordHasher.HashPassword(secondUser, "Yaqoob@12");
                thridUser.PasswordHash = thridUserHashedPassword;

                await userManager.CreateAsync(thridUser);
            }
        }
    }
}

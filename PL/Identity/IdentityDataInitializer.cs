using System;
using BLL.Models;
using Microsoft.AspNetCore.Identity;

namespace PL.Identity
{
    public static class IdentityDataInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                var role = new ApplicationRole
                {
                    Name = "Employee"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Manager").Result)
            {
                var role = new ApplicationRole
                {
                    Name = "Manager"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new ApplicationRole
                {
                    Name = "Administrator"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            Guid firstUserId = Guid.Parse("33293875-AB05-495D-8883-8C23BFB66C2C"),
                secondUserId = Guid.Parse("546C8C7C-4A94-464B-8CC7-FFFB35BB0D29"),
                thirdUserId = Guid.Parse("1E547CAB-DBED-41AA-95C9-4AFC3BA183A4"),
                fourthUserId = Guid.Parse("07D28657-02A1-47AD-BE2F-7D0E0B4E45B1");

            if (userManager.FindByNameAsync("AdminUser").Result == null)
            {
                var user = new ApplicationUser
                {
                    Id = firstUserId,
                    UserName = "AdminUser",
                    Email = "admin@gmail.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "password").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }

            if (userManager.FindByNameAsync("ManagerUser").Result == null)
            {
                var user = new ApplicationUser
                {
                    Id = secondUserId,
                    UserName = "ManagerUser",
                    Email = "manager@gmail.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "password").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                }
            }

            if (userManager.FindByNameAsync("EmployeeUser").Result == null)
            {
                var user = new ApplicationUser
                {
                    Id = thirdUserId,
                    UserName = "EmployeeUser",
                    Email = "employee@gmail.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "password").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }

            if (userManager.FindByNameAsync("SecondEmployeeUser").Result == null)
            {
                var user = new ApplicationUser
                {
                    Id = fourthUserId,
                    UserName = "SecondEmployeeUser",
                    Email = "email@gmail.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "password").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
        }
    }
}

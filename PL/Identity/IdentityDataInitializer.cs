﻿using System;
using BLL.Models;
using Microsoft.AspNetCore.Identity;

namespace PL.Identity
{
    /// <summary>
    /// Adds test data about application users to the database.
    /// </summary>
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
                roleManager.CreateAsync(role).Wait();
            }

            if (!roleManager.RoleExistsAsync("Manager").Result)
            {
                var role = new ApplicationRole
                {
                    Name = "Manager"
                };
                roleManager.CreateAsync(role).Wait();
            }

            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new ApplicationRole
                {
                    Name = "Administrator"
                };
                roleManager.CreateAsync(role).Wait();
            }
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            var userIds = new Guid[]
            {
                Guid.Parse("33293875-AB05-495D-8883-8C23BFB66C2C"),
                Guid.Parse("546C8C7C-4A94-464B-8CC7-FFFB35BB0D29"),
                Guid.Parse("1E547CAB-DBED-41AA-95C9-4AFC3BA183A4"),
                Guid.Parse("07D28657-02A1-47AD-BE2F-7D0E0B4E45B1")
            };

            if (userManager.FindByNameAsync("AdminUser").Result == null)
            {
                var user = new ApplicationUser
                {
                    Id = userIds[0],
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
                    Id = userIds[1],
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
                    Id = userIds[2],
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
                    Id = userIds[3],
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

using Microsoft.AspNetCore.Identity;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Data
{
    public class SeedData
    {
        public async Task Initialize(IServiceProvider serviceProvider, RoleManager<Role> roleManager, UserManager<Account> userManager)
        {
            await InitializeRole(roleManager);
            await InitializeAccount(roleManager, userManager);
        }

        private static async Task InitializeRole(RoleManager<Role> roleManager)
        {
            var roles = Enum.GetNames(typeof(RoleEnums));
            foreach (var roleName in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new Role(roleName, "nothing", true, DateTime.UtcNow.ToLocalTime(), "system", false));
                }
            }
        }

        private static async Task InitializeAccount(RoleManager<Role> roleManager, UserManager<Account> userManager)
        {
            var adminRole = await roleManager.FindByNameAsync(RoleEnums.Admin.ToString());
            if (adminRole != null)
            {
                var adminUser = new Account
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    CreatedAt = DateTime.UtcNow.ToLocalTime(),
                    CreatedBy = "system",
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole.Name);
                }
            }

            var staffRole = await roleManager.FindByNameAsync(RoleEnums.Staff.ToString());
            if (staffRole != null)
            {
                var staffUser = new Account
                {
                    UserName = "staff",
                    Email = "staff@example.com",
                    CreatedAt = DateTime.UtcNow.ToLocalTime(),
                    CreatedBy = "system",
                };
                var result = await userManager.CreateAsync(staffUser, "Staff@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(staffUser, staffRole.Name);
                }
            }

            var customerRole = await roleManager.FindByNameAsync(RoleEnums.Customer.ToString());
            if (customerRole != null)
            {
                var customerUser = new Account
                {
                    UserName = "customer",
                    Email = "customer@example.com",
                    CreatedAt = DateTime.UtcNow.ToLocalTime(),
                    CreatedBy = "system",
                };
                var result = await userManager.CreateAsync(customerUser, "Customer@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, customerRole.Name);
                }
            }
        }

    }
}

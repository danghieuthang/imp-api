using IMP.Application.Enums;
using IMP.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Check if roles aldready Exists Before seeding otherwise we get warning everytime project starts
            if (!roleManager.Roles.Any())
            {
                //Seed Roles
                await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Brand.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Influencer.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Fan.ToString()));
            }
        }
    }
}

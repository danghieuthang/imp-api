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
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new User
            {
                UserName = "basicuser",
                Email = "basicuser@gmail.com",
                FirstName = "John",
                LastName = "Doe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            //Seed Brand User
            var brandUser = new User
            {
                UserName = "branduser",
                Email = "branduser@gmail.com",
                FirstName = "John",
                LastName = "Doe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            //Seed Influencer User

            var influencerUser = new User
            {
                UserName = "influenceruser",
                Email = "influenceruser@gmail.com",
                FirstName = "John",
                LastName = "Doe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Fan.ToString());
                }

                user = await userManager.FindByEmailAsync(brandUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(brandUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(brandUser, Roles.Brand.ToString());
                }

                user = await userManager.FindByEmailAsync(influencerUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(influencerUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(influencerUser, Roles.Influencer.ToString());
                }

            }
        }
    }
}

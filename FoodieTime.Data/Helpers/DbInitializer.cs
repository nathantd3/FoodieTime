using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodieTime.Data.Models;

namespace FoodieTime.Data.Helpers
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext appDbContext)
        {
            if(!appDbContext.Users.Any() && !appDbContext.Posts.Any())
            {
                var newUser = new User()
                {
                    FullName = "Nathan Duong",
                    ProfilePictureUrl = ""
                };
                await appDbContext.Users.AddAsync(newUser);
                await appDbContext.SaveChangesAsync();

                var newPostWithoutImage = new Post()
                {
                    Content = "The Panda Express orange chicken was absolutely delicious",
                    ImageUrl = "",
                    NrOfReports = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    Dish = "Orange Chicken",
                    Rating = 5,
                    Restaurant = "Panda Express",
                    UserId = newUser.Id
                };

                var newPostWithImage = new Post()
                {
                    Content = "The chow mein is something you cannot skip. I get it every single time",
                    ImageUrl = "https://scontent-lax3-1.xx.fbcdn.net/v/t39.30808-6/468554249_10162218184893814_2601455031211097591_n.jpg?_nc_cat=104&ccb=1-7&_nc_sid=127cfc&_nc_ohc=uwA8APe1cpoQ7kNvgHecpTp&_nc_oc=Adi3YnSGC-xdkbOndWz14umxw1br-VRa-ujPd2SPVjOafgyUEPqajlYjPUyrxwE_Ai9TFBZ41lLKUPcs-ycRtwZP&_nc_zt=23&_nc_ht=scontent-lax3-1.xx&_nc_gid=Ahv4C_-Svb311OabizZ330q&oh=00_AYCtm0qizzXQ0wybaKw92xxx2cqlva88eAx8nM82KGNbmQ&oe=67BDDB7A",
                    NrOfReports = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    Dish = "Chow Mein",
                    Rating = 5,
                    Restaurant = "Panda Express",
                    UserId = newUser.Id
                };

                await appDbContext.Posts.AddRangeAsync(newPostWithoutImage, newPostWithImage);
                await appDbContext.SaveChangesAsync();
            }

        }
    }
}

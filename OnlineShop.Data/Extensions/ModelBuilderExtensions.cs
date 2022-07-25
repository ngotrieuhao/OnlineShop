using OnlineShop.Data.Entities;
using OnlineShop.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
               new AppConfig() { Key = "HomeTitle", Value = "This is home page of OnlineShop" },
               new AppConfig() { Key = "HomeKeyword", Value = "This is keyword of OnlineShop" },
               new AppConfig() { Key = "HomeDescription", Value = "This is description of OnlineShop" }
               );
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = "en", Name = "English", IsDefault = false });

            modelBuilder.Entity<Category>().HasData(
               new Category()
               {
                   Id = 1,
                   Name = "Adidas"
               },

               new Category()
               {
                   Id = 2,
                   Name = "Nike"
               },
                new Category()
                {
                    Id = 3,
                    Name = "Puma"
                }
             );


            modelBuilder.Entity<Product>().HasData(
                 new Product()
                 {
                     Id = 1,
                     Name = "Adidas ULTRABOOST 21",
                     CategoryId = 1,
                     Price = 250,
                     Stock = 15,
                     DateCreated = DateTime.Now,
                     Description = "",
                     Details = ""
                 },

                new Product()
                {
                    Id = 2,
                    Name = "NIKE WEARALLDAY",
                    CategoryId = 2,
                    Price = 150,
                    Stock = 25,
                    DateCreated = DateTime.Now,
                    Description = "",
                    Details = ""
                },

                new Product()
                {
                    Id = 3,
                    Name = "Puma Cali Star Metallic",
                    CategoryId = 3,
                    Price = 250,
                    Stock = 35,
                    DateCreated = DateTime.Now,
                    Description = "",
                    Details = ""
                }
                );
           
            // any guid
            var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            modelBuilder.Entity<AppRole>().HasData(
            new AppRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "ngotrieuhao@gmail.com",
                PhoneNumber = "0944334052",
                Address = "315/12 Tùng Thiện Vương",
                NormalizedEmail = "ngotrieuhao@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Hao123@"),
                SecurityStamp = string.Empty,
                Name = "Ngo Hao",
            });

            // gán role admin và admin user
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });
        }
    }
}

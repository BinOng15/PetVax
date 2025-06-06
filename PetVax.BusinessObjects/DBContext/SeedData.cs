using Microsoft.EntityFrameworkCore;
using PetVax.BusinessObjects.Helpers;
using PetVax.BusinessObjects.Models;
using System;

namespace PetVax.BusinessObjects.DBContext
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            string adminSalt = PasswordHelper.GenerateSalt();
            string adminHash = PasswordHelper.HashPassword("Admin@123", adminSalt);

            string staffSalt = PasswordHelper.GenerateSalt();
            string staffHash = PasswordHelper.HashPassword("Staff@123", staffSalt);

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountId = 1,
                    Email = "admin@petvax.com",
                    PasswordHash = adminHash,
                    PasswordSalt = adminSalt,
                    Role = Enum.EnumList.Role.Admin,
                    AccessToken = "",
                    RefereshToken = "",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system",
  
                },
                new Account
                {
                    AccountId = 2,
                    Email = "staff@petvax.com",
                    PasswordHash = staffHash,
                    PasswordSalt = staffSalt,
                    Role = Enum.EnumList.Role.Staff,
                    AccessToken = "",
                    RefereshToken = "",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system",

                }
            );
        }
    }
}

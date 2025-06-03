using Microsoft.EntityFrameworkCore;
using PetVax.BusinessObjects.Models;
using System;

namespace PetVax.BusinessObjects.DBContext
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountId = 1,
                    Email = "admin@petvax.com",
                    Password = "admin123", // Nên hash mật khẩu trong thực tế!
                    Role = Enum.EnumList.Role.Admin,
                    AccessToken = "",
                    RefereshToken = "",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system",
                    ModifiedAt = null,
                    ModifiedBy = ""
                }
            );
        }
    }
}

﻿using Microsoft.EntityFrameworkCore;
using PetVax.BusinessObjects.Helpers;
using PetVax.BusinessObjects.Models;
using System;

namespace PetVax.BusinessObjects.DBContext
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            string salt = PasswordHelper.GenerateSalt();
            string hashedAdmin = PasswordHelper.HashPassword("admin123", salt);
            string hashedStaff = PasswordHelper.HashPassword("staff123", salt);

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountId = 1,
                    Email = "admin@petvax.com",
                    Password = hashedAdmin,
                    Role = Enum.EnumList.Role.Admin,
                    AccessToken = "",
                    RefereshToken = "",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system",
                    ModifiedAt = null,
                    ModifiedBy = ""
                },
                new Account
                {
                    AccountId = 2,
                    Email = "staff@petvax.com",
                    Password = hashedStaff,
                    Role = Enum.EnumList.Role.Staff,
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

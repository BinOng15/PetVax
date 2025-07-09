using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVaccineId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 18, 21, 17, 296, DateTimeKind.Utc).AddTicks(7126), "IQWNeACzQx5mE2i03VmE3Q6ZxTQGaYp85cZ82cIUhJs=", "Wxawgk9jzw5dCWhRyKeW9eSYWd3cuij9Y21fTiyryWY=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 18, 21, 17, 296, DateTimeKind.Utc).AddTicks(7135), "MsvWdniSFGQV0PnRGqeirjMEZBOdJ6iWQ27UvMpMlTA=", "g1Vk8C6XimmuYmIACQYSgGb4HWDMTvSHu+mwLDFdRH8=" });

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Vaccine_VaccineId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_VaccineId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "VaccineId",
                table: "Payment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 18, 17, 38, 932, DateTimeKind.Utc).AddTicks(3365), "jDSHkoK5HZ/Fj8hQUbIQVu7/kLmUobPbKphWKycXYD4=", "SiCr9qmvvDkBjiP+gPuMdbIMG1eVZnHU7knULXLO148=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 18, 17, 38, 932, DateTimeKind.Utc).AddTicks(3370), "mpqLudyUBNiS09Yxi2UO89XS1u4t44wjEjulokoyiT4=", "u+DUTXWlTxKjUGiS7YPtR+4t0p0DWf5Ner8Ppiv5mLI=" });
        }
    }
}

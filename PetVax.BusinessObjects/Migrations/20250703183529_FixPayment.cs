using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class FixPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 18, 35, 28, 270, DateTimeKind.Utc).AddTicks(7178), "5XN27ZsWplazDRATpSN3kUEZJmu96EBygDmgClu+Jok=", "3Z9g4lQIg80RR8VG5yje2fpQ/Zfl++0STS/vbmUQbtQ=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 18, 35, 28, 270, DateTimeKind.Utc).AddTicks(7183), "AG3wiQ7Xq5CIYi4TrDyVHi/CwGKT++vHJB52zIS2nVM=", "n8DSejdvgX4M4iBPhM4WJT5jVzpGJctOOB3tkjvDDlA=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VaccineId",
                table: "Payment",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Payment_VaccineId",
                table: "Payment",
                column: "VaccineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Vaccine_VaccineId",
                table: "Payment",
                column: "VaccineId",
                principalTable: "Vaccine",
                principalColumn: "VaccineId");
        }
    }
}

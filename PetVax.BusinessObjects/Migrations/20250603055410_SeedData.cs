using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Account",
                columns: new[] { "AccountId", "AccessToken", "CreatedAt", "CreatedBy", "Email", "ModifiedAt", "ModifiedBy", "Password", "RefereshToken", "Role" },
                values: new object[] { 1, "", new DateTime(2025, 6, 3, 5, 54, 10, 322, DateTimeKind.Utc).AddTicks(1892), "system", "admin@petvax.com", null, "", "admin123", "", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1);
        }
    }
}

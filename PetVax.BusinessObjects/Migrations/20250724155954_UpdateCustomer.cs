using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RedeemablePoints",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 15, 59, 53, 721, DateTimeKind.Utc).AddTicks(8006), "JF0GVOF6O/fkBzQR1mfxk9GDEkgtKHYqJeWqBlfEp+s=", "DZ0xbFMRfgEUDxclXefm3pLBuIscESg3ASH0P0yBZ9c=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 15, 59, 53, 721, DateTimeKind.Utc).AddTicks(8010), "Cz+LAhMCrYofzHNYgR1HW21DC5f4+vapn9aLSpDlo3U=", "TOhJThrmGfiP60S3yWbZndNiwfIfHi6nKJTr8QXiw9U=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedeemablePoints",
                table: "Customer");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 14, 22, 17, 118, DateTimeKind.Utc).AddTicks(2564), "vjrOyijB70Nwz7f6zuNAebOW258VXpEWpo4dS6HjwE8=", "zo0Zwi8FZfhN7R7dNFTPFpI9pwJeJhmiB9d2m9620a0=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 14, 22, 17, 118, DateTimeKind.Utc).AddTicks(2568), "YefIM0pUQ0FoCEUKHTdKMFFK71FhGIQJFF/K5l4FYQk=", "ohkzlNgMZ3H0n0uiOyCFzZAHN44O4v1nNPrve5VXNGg=" });
        }
    }
}

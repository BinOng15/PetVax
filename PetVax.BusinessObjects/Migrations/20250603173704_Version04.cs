using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class Version04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                schema: "dbo",
                table: "Account",
                newName: "PasswordSalt");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                schema: "dbo",
                table: "Account",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 3, 17, 37, 4, 295, DateTimeKind.Utc).AddTicks(8297), "DDRb7Hr/iNbgVTlZnkn7lA==", "WS5sT/iN9eoxV0zKcs6cvA==" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 3, 17, 37, 4, 295, DateTimeKind.Utc).AddTicks(8301), "BT3hbcDQ+WtYlrUOrDp1xw==", "WS5sT/iN9eoxV0zKcs6cvA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                schema: "dbo",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                schema: "dbo",
                table: "Account",
                newName: "Password");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 6, 3, 16, 53, 35, 407, DateTimeKind.Utc).AddTicks(4722), "MGQ7oohWqVQBsdRh0UqR+QYd2QCulMP65rt2WHqdusQ=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 6, 3, 16, 53, 35, 407, DateTimeKind.Utc).AddTicks(4731), "RqwTQyZQJPzICWex0yhR4d8d20cc7CNarVgVxmpdI9Q=" });
        }
    }
}

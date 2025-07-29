using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class update_databaseServiceHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "ServiceHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 25, 2, 56, 19, 793, DateTimeKind.Utc).AddTicks(9327), "RqCqnvnFkWNJRG9ISywMAvrERA8/lRSD+WoGMhuesEA=", "gpvVsYDcnNOYf1aSKk9PUT6aNSosjiiG4fQsXzFIaG0=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 25, 2, 56, 19, 793, DateTimeKind.Utc).AddTicks(9332), "WepxuLTfahA3ocX3Zt46nINyjWu7hiflieLOrK9P2/0=", "9z/McodwTBpWA2296O5ouiB1fkQv26a+crixrD8z46g=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ServiceHistory");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 22, 11, 46, 719, DateTimeKind.Utc).AddTicks(6347), "dICR9HXf7zCNpNJ9qm90SzNwfNqfKCcozJXJ8v9PbKo=", "URS7o/6SHJOGD38rUyKOgYp1E1bkzqEuTp9dX8o7ieo=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 22, 11, 46, 719, DateTimeKind.Utc).AddTicks(6356), "edMwVvSSl4kT9fL7nJNQj2hZ3+2GgjoJI8aGf3s2h+M=", "Y1DjBrmMxRNhPNtVWWAuKLsBWjpgqFgNkzd1RaXfvEc=" });
        }
    }
}

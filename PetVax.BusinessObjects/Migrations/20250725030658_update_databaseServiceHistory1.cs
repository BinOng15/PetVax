using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class update_databaseServiceHistory1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 25, 3, 6, 57, 642, DateTimeKind.Utc).AddTicks(1748), "cztZD1B1MXEB8ZZieaQCRZNcvWsDUJ3AnCyS3tOhuFM=", "7Alw0w8dXgZPeBKUzKxXW4iIu8uUcDQt/K0QH1nrfXs=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 25, 3, 6, 57, 642, DateTimeKind.Utc).AddTicks(1752), "nbOV4t43aBkimhtaScav3UAvnMDtzHtcUI+KjHLKKCA=", "omRCvnDC9EEDhqUOlyFf2vl/kS7eVsKqGjiWTdOb0QE=" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistory_CustomerId",
                table: "ServiceHistory",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceHistory_Customer_CustomerId",
                table: "ServiceHistory",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceHistory_Customer_CustomerId",
                table: "ServiceHistory");

            migrationBuilder.DropIndex(
                name: "IX_ServiceHistory_CustomerId",
                table: "ServiceHistory");

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
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class Url : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CheckoutUrl",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 19, 5, 25, 849, DateTimeKind.Utc).AddTicks(3460), "7s7586izJjEt5jlAObLN5Jghhmt22SSP1pIno7B95oY=", "FHBf5diMjz5sxTxkxaLNIFHNG66F/LX97FwyuVLLZ58=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 19, 5, 25, 849, DateTimeKind.Utc).AddTicks(3466), "aU+Lq1wS6IOaZ0KvgwrCu8DU2V8+qXcoJyChm/xeLRk=", "bGdNbmQD4Y9bb0SPtCjMnIOv7eoX1mUQImVPojpHeRI=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckoutUrl",
                table: "Payment");

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
    }
}

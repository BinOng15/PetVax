using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class AddVoucherCodeToPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VoucherCode",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoucherCode",
                table: "Payment");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 21, 8, 30, 579, DateTimeKind.Utc).AddTicks(7820), "Sai24HCjZnOKw2KjVHIva3UXXV4QkXIyjhbDV5u20Hg=", "cSFsUgUlmPcaoEvM8EMBKoIYaKMyfv5KIr8CxBLUf4Q=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 21, 8, 30, 579, DateTimeKind.Utc).AddTicks(7831), "WPL+KyOFqQ/VKuP8rT2mYgGac3x8ApNYgQWCHuaGuEg=", "NTseK+ye6f2KYWE2aJKXoNjGFQsX5R5Di+7dSEZx6nY=" });
        }
    }
}

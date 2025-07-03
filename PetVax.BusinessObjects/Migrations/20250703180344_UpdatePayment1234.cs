using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePayment1234 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 18, 3, 43, 992, DateTimeKind.Utc).AddTicks(4766), "Ffo0V0CwD0TfcKmjxh9MPKeodOBBKuXoiEHAX8HEAps=", "DPcvJF7B8lpuBPUKNEU83WvxHwbN4EwcgYUFLZ6nwXQ=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 18, 3, 43, 992, DateTimeKind.Utc).AddTicks(4771), "o8KKmpfkgW+g5KVGL4+jVtxo/GfHxUU5RkD4a9VkbEI=", "RJLR/f9OYswvr0u+/0Ln3k/g5THt5XPexTO2TomaBrE=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 17, 51, 44, 621, DateTimeKind.Utc).AddTicks(59), "AM+Cqg+T93Yd3EffVBL0W1NXV0z1KgUFLV/lcXkp444=", "xfEnO0qamPzhKZwjAfASnMG66h6rbTzKcgw2dfP6L04=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 17, 51, 44, 621, DateTimeKind.Utc).AddTicks(66), "EguLF/Cx1IAnwVqFTY8a/Qpku989sg1jUqelxAbVdp8=", "H4LcLV5liB5Nxd4Owq0w72edkofy/+AFVpUJmd7yGkE=" });
        }
    }
}

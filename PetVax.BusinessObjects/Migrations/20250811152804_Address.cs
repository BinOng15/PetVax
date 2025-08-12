using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class Address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 11, 15, 28, 4, 129, DateTimeKind.Utc).AddTicks(1868), "lG8mJYUsQYbkSJlNCb2lW3KUX4kByEq2Atl46+vBf6A=", "SjfvbytGb122RPQBbceUugeIj3wRHbMdx9NcY6DEz6s=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 11, 15, 28, 4, 129, DateTimeKind.Utc).AddTicks(1872), "HoPNCduxOV6chlJGtRkd4yudjqdNx5lpz6YEdTx8z8Y=", "SdaMAZwaT/78KO0MDYXBNe11xl0+zAZ0xhEcqOjkANE=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 5, 18, 11, 52, 378, DateTimeKind.Utc).AddTicks(4470), "/jVB/Uod6JAEqlallfXLo+1bhTIJYZeoy7VD/ohyHGE=", "vfcAu6oZoJhzrEpbZrCPlk9D4IId3Wdq7AOJ1srm9Ys=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 5, 18, 11, 52, 378, DateTimeKind.Utc).AddTicks(4479), "nXRB/EgbEq2i2tse76eOwmCRaQzubu1SRvL+mhUp1JQ=", "qarbrrtrWreYFt4ig8Wm8TgXq9XdBaKz2l8kbfdcuVY=" });
        }
    }
}

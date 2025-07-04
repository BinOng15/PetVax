using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 19, 10, 57, 7, 977, DateTimeKind.Utc).AddTicks(8519), "01tS6PniA1hQiJJrG3/hADEAUr/gyUPr3+7O/LMD1NM=", "LZhb66Yvq8eaxvs96UnsMZz3FI3ecMo+3jxfJaE59VE=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 19, 10, 57, 7, 977, DateTimeKind.Utc).AddTicks(8524), "FrzcuT197CSvxc8bD3kUWxycIXBMCCm7J9m7FsJ/m74=", "syNH9manwFl15pmlMe5T5ErQJY+gxCHIUdelw4BXr1M=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 19, 10, 55, 28, 587, DateTimeKind.Utc).AddTicks(5167), "xIu4NemvqLdEtU40EPl44UKNhrZcj6DpsBCyw9eyWpQ=", "3UJRcAX3dsY/D/9kKUZYEMK7YyqWOBFo8kWVPioxdGg=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 19, 10, 55, 28, 587, DateTimeKind.Utc).AddTicks(5174), "G7/bf76wjOAOzwCXBzHaFPvyu/Bz0VCFv4rGBpiaJzQ=", "nyYa7Kckc/UaUtrhQbXr/koPuUA5So1XlbVI2Nw8src=" });
        }
    }
}

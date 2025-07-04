using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class updateVetschedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "VetSchedule",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 19, 13, 52, 21, 227, DateTimeKind.Utc).AddTicks(6232), "YQ7id8GLiyD5+PRRirkre22mHbi5cCLHVOiX26+ap0U=", "9J2pG6R6nbWtl4mKTrqf6/WT0ACbtYJVG6nLTffeLPA=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 19, 13, 52, 21, 227, DateTimeKind.Utc).AddTicks(6236), "90EYaEPp0F1H/3NCx94teSv73p41+NMCv/9IUlM6Hms=", "dQo+8kwoakfTAMCzwjk0d63CQMY9dk79EcxoH/sB74g=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "VetSchedule",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}

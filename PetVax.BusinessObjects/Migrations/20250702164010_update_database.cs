using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class update_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MicrochipItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InstallationDate",
                table: "MicrochipItem",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MicrochipItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 2, 16, 40, 9, 517, DateTimeKind.Utc).AddTicks(9842), "rfmSbjG0/9bdjywjX9IQvosXiIYETkIY/LW8AZTjPLA=", "cwWXaJKNX+FpYuYESNpyfqW0rNi+QMxFLtj8bM/FYJg=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 2, 16, 40, 9, 517, DateTimeKind.Utc).AddTicks(9845), "Mq6Y7i6zpvcLmD1OZbiZwa81z4jeklUX5ZnGaVBdZPc=", "vFO3pf1SZVP5r7U/hHycbJeF4Rv5laYnRtjoI60vEnE=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MicrochipItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InstallationDate",
                table: "MicrochipItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MicrochipItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 28, 3, 20, 7, 250, DateTimeKind.Utc).AddTicks(7662), "lusxvYfKNMavUZlW09aXjobS9gukrR2sLYIU819MhbA=", "VyR+Wycf9GZk+uAYNz1zN7z+ePxaPP38n53GjoDz9gg=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 28, 3, 20, 7, 250, DateTimeKind.Utc).AddTicks(7667), "AQwFp7Y/2ZSJbilBkqDni69480oJ5YS+bodL5c1zWPw=", "LXqaMm0/2q8BjhoFrVccbfAgY9yZXEJ/GlEcdUvecuM=" });
        }
    }
}

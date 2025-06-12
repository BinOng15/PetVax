using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class deploy_ver10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                schema: "dbo",
                table: "Pet",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 12, 13, 56, 58, 895, DateTimeKind.Utc).AddTicks(7601), "9Qbsyb1V7/w7XI4UlQDmWHAVr+/GJ/X0Q8oYsbhC2Bs=", "ZJ95VL6xis8Ln4rOtXnPQ/Ns0rpXwSOdPzYqiRSwqtI=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 12, 13, 56, 58, 895, DateTimeKind.Utc).AddTicks(7606), "9+F37WRPGyFNSaLMUfKXLh7IG2DQkYzNfQuxkZuy9wg=", "/3Ja7vzphpOujeZhz+uJlOU9OpdeLMaOQyZVb0UbY+w=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                schema: "dbo",
                table: "Pet",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 12, 8, 24, 44, 896, DateTimeKind.Utc).AddTicks(7503), "wxQenlR9W/AJvvNlYhXC630oAxYTL3Oww76Fn+lskJA=", "pb8WBRrEDn8QS3dG1ZZtC4BivdR7p/15kO++RqtPt6w=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 12, 8, 24, 44, 896, DateTimeKind.Utc).AddTicks(7508), "M+k2Jd6GypMEIBus5JwH8CuY698ZE41ifnlDdxJnwrk=", "rbVSwa5vd9Dh4kqDpWHW6KtKI+laghz+CsOJeaGqNEg=" });
        }
    }
}

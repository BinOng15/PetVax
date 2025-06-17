using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVer11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "dbo",
                table: "Appointment",
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
                values: new object[] { new DateTime(2025, 6, 17, 8, 28, 46, 774, DateTimeKind.Utc).AddTicks(7690), "Uu/X1Cxh/G3N0oGuHtVS3uRalzSRPLavXIKUubWlF/E=", "zFkRw68p5FVa1waTJoJr2PsaOhj0bzUUBCR17924opw=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 17, 8, 28, 46, 774, DateTimeKind.Utc).AddTicks(7696), "Zwps6hMGVQwP+X2e1MHeG5+H34bNb80OZVKtg9KGkBU=", "xHtkFu8xzi1mYYgNu6chfwsM3okkmULiY4I7APfQWeE=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "dbo",
                table: "Appointment",
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
                values: new object[] { new DateTime(2025, 6, 17, 8, 4, 9, 409, DateTimeKind.Utc).AddTicks(1073), "OKH0rFytz0cylyw8DIt6y0/SgCbBRvRWaa8tSIu+fCA=", "NFMR4vLj3fV1IssDiXgTeGnOLa/Ts6jLnYO7fSugiLk=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 17, 8, 4, 9, 409, DateTimeKind.Utc).AddTicks(1080), "Y3SlYdodVBdg81ym3qTZwernchZjR/gOf1t2KIpJnME=", "scTPVo2gISVysvF4dTBKHMVI2hBBuORmAa+ADAOlz4w=" });
        }
    }
}

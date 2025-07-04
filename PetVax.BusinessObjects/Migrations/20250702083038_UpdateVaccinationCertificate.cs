using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVaccinationCertificate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MicrochipItemId",
                table: "VaccinationCertificate",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 30, 37, 769, DateTimeKind.Utc).AddTicks(7548), "sfkVs/eYs/qNgSkfHGWT85B1qUOB3upfUziIoUVbVj0=", "VzWKwMLQxv3Hg6iOSyuXn4pI/blXSNHfk5pXmI6MStM=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 30, 37, 769, DateTimeKind.Utc).AddTicks(7584), "v+1t7AlFaakhtjbfTChyTqtaSJtw1RzwWA45NWN3wIY=", "2KYS+rfjURV5STbO44olZ5ZcfyiLThLDoAExT+ByI6E=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MicrochipItemId",
                table: "VaccinationCertificate",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 19, 50, 19, DateTimeKind.Utc).AddTicks(6213), "/I2qqoaqPcjJDuycDNo4IlLrKolZTGUn9LWz41DnXdA=", "hU5FdUdzNr+OUWMT3iwdE3pt7h/pXfv6QVNcNWRyeto=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 19, 50, 19, DateTimeKind.Utc).AddTicks(6220), "3jtuEowq4CaYH5BDjodBdzqP7J8xYAuSq9H/kU3jkD8=", "6YE9NdBBEKc6QDmoJn/dfRV5BhBuWPd6pjan+Zqcnw4=" });
        }
    }
}

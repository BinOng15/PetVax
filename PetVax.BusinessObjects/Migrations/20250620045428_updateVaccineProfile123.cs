using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class updateVaccineProfile123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VaccineProfileId",
                table: "VaccineProfileDisease",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DiseaseId",
                table: "VaccineProfileDisease",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 20, 4, 54, 27, 810, DateTimeKind.Utc).AddTicks(8406), "8VzVC8ZkIKucKf3PLFSWELantOpEDs13PZuntFtTh+E=", "bRAeKdzBVyMwe5HEEL7ZHXcvFy4ZP34parKCeM5zzeI=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 20, 4, 54, 27, 810, DateTimeKind.Utc).AddTicks(8411), "+Dt2sQ5ZEIwA393R0hDJKrhlAYPiPutwrA2ISA5tveE=", "PaxPOQo1d3lG18iW3dwXsoNM5fOMujAfVJkVrEgQj3Y=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VaccineProfileId",
                table: "VaccineProfileDisease",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DiseaseId",
                table: "VaccineProfileDisease",
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
                values: new object[] { new DateTime(2025, 6, 20, 4, 18, 27, 909, DateTimeKind.Utc).AddTicks(4115), "Dgfh2wQOJnNC/frYzubEQ4tolWjTSknB5QbU/g/DwVI=", "S/B15Dte8OS1ebk+LejlI1mxntSTwmdBiCFOwmXlOtI=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 20, 4, 18, 27, 909, DateTimeKind.Utc).AddTicks(4123), "CscrmXOnndGByCni+eD80fnPGq0fqmvjTghwEx7udQM=", "sgcrtpJdOEYZwcV7oJIJ586gAECI7oGFvKlsFeqH9/8=" });
        }
    }
}

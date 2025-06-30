using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class updateVaccineProfile12345 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccineProfileDisease_Disease_DiseaseId",
                table: "VaccineProfileDisease");

            migrationBuilder.DropForeignKey(
                name: "FK_VaccineProfileDisease_VaccineProfile_VaccineProfileId",
                table: "VaccineProfileDisease");

            migrationBuilder.AddColumn<int>(
                name: "DiseaseId",
                table: "VaccineProfile",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 24, 15, 25, 40, 925, DateTimeKind.Utc).AddTicks(4300), "yfDaWzxNt9PyOhTy2MKX0CuiYnk1c6/Napm5hCl6Dq4=", "jGhaOr0O2LyAFs27j+m6Xdzu005X1ZVY/dJ9SjNE4UM=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 24, 15, 25, 40, 925, DateTimeKind.Utc).AddTicks(4305), "AzepCFQwth70mpYieLLfv24IW4ogFGKSYCFTasrs8Tk=", "idlwWhPm4dPgnIko2hSk5QDoSRiPlFYd4tvptNM0k5Y=" });

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfile_DiseaseId",
                table: "VaccineProfile",
                column: "DiseaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineProfile_Disease_DiseaseId",
                table: "VaccineProfile",
                column: "DiseaseId",
                principalTable: "Disease",
                principalColumn: "DiseaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineProfileDisease_Disease_DiseaseId",
                table: "VaccineProfileDisease",
                column: "DiseaseId",
                principalTable: "Disease",
                principalColumn: "DiseaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineProfileDisease_VaccineProfile_VaccineProfileId",
                table: "VaccineProfileDisease",
                column: "VaccineProfileId",
                principalTable: "VaccineProfile",
                principalColumn: "VaccineProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccineProfile_Disease_DiseaseId",
                table: "VaccineProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_VaccineProfileDisease_Disease_DiseaseId",
                table: "VaccineProfileDisease");

            migrationBuilder.DropForeignKey(
                name: "FK_VaccineProfileDisease_VaccineProfile_VaccineProfileId",
                table: "VaccineProfileDisease");

            migrationBuilder.DropIndex(
                name: "IX_VaccineProfile_DiseaseId",
                table: "VaccineProfile");

            migrationBuilder.DropColumn(
                name: "DiseaseId",
                table: "VaccineProfile");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 23, 18, 14, 23, 219, DateTimeKind.Utc).AddTicks(8895), "zPUWUxm7CKyFi9qdurAgfFG41JNG0wbMMGTlUomUvw0=", "A6SPaEm4qnfS4c/mnnVjKp0SVyP54Mx9YfNV8Ykxh1o=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 23, 18, 14, 23, 219, DateTimeKind.Utc).AddTicks(8900), "4dsHrVk0EE5YOIJUJZ1nDqOp3QUtW8k4IolYCdVJddk=", "xXxKWQylEX0Ej+PpITNZG+Uwvf+t5rRRie8tpAihcZE=" });

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineProfileDisease_Disease_DiseaseId",
                table: "VaccineProfileDisease",
                column: "DiseaseId",
                principalTable: "Disease",
                principalColumn: "DiseaseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineProfileDisease_VaccineProfile_VaccineProfileId",
                table: "VaccineProfileDisease",
                column: "VaccineProfileId",
                principalTable: "VaccineProfile",
                principalColumn: "VaccineProfileId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

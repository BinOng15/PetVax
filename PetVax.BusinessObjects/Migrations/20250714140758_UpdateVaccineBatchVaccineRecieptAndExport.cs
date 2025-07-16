using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVaccineBatchVaccineRecieptAndExport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "VaccineReceipt");

            migrationBuilder.DropColumn(
                name: "Suppiler",
                table: "VaccineReceipt");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "VaccineExport");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "VaccineExport");

            migrationBuilder.AddColumn<string>(
                name: "Suppiler",
                table: "VaccineReceiptDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VaccineStatus",
                table: "VaccineReceiptDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "VaccineExportDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "VaccineBatch",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "VaccineBatch",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StorageConditions",
                table: "VaccineBatch",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 14, 14, 7, 57, 328, DateTimeKind.Utc).AddTicks(5898), "q+Bpi464Myl/C3It5spFviug3I9GZQ31V9wlsN98nh8=", "bbhKcrBuzFd7Gdci2ZKWylXRWgPGjxlsRGYkBS0TxiA=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 14, 14, 7, 57, 328, DateTimeKind.Utc).AddTicks(5909), "ZGqWYaNSpIlVdEQAj7nSJP194KDm966dFGcilKrV5Rw=", "z904tRcV+ZFr8Yru8+n/2L2e/VMy4T8Kc4PLxYcwj90=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Suppiler",
                table: "VaccineReceiptDetail");

            migrationBuilder.DropColumn(
                name: "VaccineStatus",
                table: "VaccineReceiptDetail");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "VaccineExportDetail");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "VaccineBatch");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "VaccineBatch");

            migrationBuilder.DropColumn(
                name: "StorageConditions",
                table: "VaccineBatch");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "VaccineReceipt",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Suppiler",
                table: "VaccineReceipt",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "VaccineExport",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "VaccineExport",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 11, 13, 55, 36, 900, DateTimeKind.Utc).AddTicks(7892), "QhVXvJTc14SwfrsYAt+d+prX2OEQM688ip8KCkpbVWA=", "KV6ozGA0yovXyqwEtPiYGLgZo3IFR8Lnz4+iEF9c+Ms=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 11, 13, 55, 36, 900, DateTimeKind.Utc).AddTicks(7905), "LIbUDoFd19JnkEm82EUySoawkcDQubSyJ3cLWNKDN4E=", "If5l1QUkI0rhUCWd7o/SJpKXe+SLCQq2Deor0aBiM5o=" });
        }
    }
}

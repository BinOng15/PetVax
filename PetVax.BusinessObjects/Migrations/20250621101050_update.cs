using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "VetSchedule",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Vet",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "VaccineReceiptDetail",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "VaccineReceipt",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "VaccineProfileDisease",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "VaccineProfile",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "VaccineExportDetail",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "VaccineExport",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "VaccineDisease",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "VaccineBatch",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Vaccine",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "VaccinationSchedule",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "ServiceHistory",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "PointTransaction",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "PetPassport",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Pet",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Payment",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "MicrochipItem",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Microchip",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Membership",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "HealthCondition",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Disease",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Customer",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "AppointmentDetail",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Appointment",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Account",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "isDeleted" },
                values: new object[] { new DateTime(2025, 6, 21, 10, 10, 49, 693, DateTimeKind.Utc).AddTicks(4957), "KKI8xAl1HG7Gy31OMdTlL02mjW7NXsDSAYxGvbaqHhg=", "iAf/a1wSceLpU/l4asEKNhysXO+zmvSzOsvXWDR/ccQ=", false });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "isDeleted" },
                values: new object[] { new DateTime(2025, 6, 21, 10, 10, 49, 693, DateTimeKind.Utc).AddTicks(4963), "a2LRW6W3fSUFF8Xob9XOctYK6237RjXO6M5a90TpmXo=", "sZoQyfJpNNy+0bZmpNNhXTLvhebZ6vI8JoXa+nK3rSk=", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "VetSchedule");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Vet");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "VaccineReceiptDetail");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "VaccineReceipt");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "VaccineProfileDisease");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "VaccineProfile");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "VaccineExportDetail");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "VaccineExport");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "VaccineDisease");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "VaccineBatch");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Vaccine");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "VaccinationSchedule");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "ServiceHistory");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "PointTransaction");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "PetPassport");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Pet");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "MicrochipItem");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Microchip");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Membership");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "HealthCondition");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Disease");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "AppointmentDetail");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Account");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 21, 10, 7, 47, 491, DateTimeKind.Utc).AddTicks(752), "UkjQaOuM3bNpMP5obR63pVt9I3b2G8BLq6ooCddcFRE=", "c19b4CSUUxRbTUOIRoOxVgKJ2jaatv8sbIBqosHIo3U=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 21, 10, 7, 47, 491, DateTimeKind.Utc).AddTicks(760), "wpsjjCo2zAh3ddMksdTnNpJXDnzEUhOu5aZrWS+HKKQ=", "ur1TPv+egL/dcKThWtS6SPlYN5F/MsSM7Xoy0WAs/I4=" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class InitialPublicSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "VetSchedule",
                newName: "VetSchedule",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Vet",
                newName: "Vet",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "VaccineReceiptDetail",
                newName: "VaccineReceiptDetail",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "VaccineReceipt",
                newName: "VaccineReceipt",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "VaccineProfile",
                newName: "VaccineProfile",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "VaccineExportDetail",
                newName: "VaccineExportDetail",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "VaccineExport",
                newName: "VaccineExport",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "VaccineDisease",
                newName: "VaccineDisease",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "VaccineBatch",
                newName: "VaccineBatch",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Vaccine",
                newName: "Vaccine",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "VaccinationSchedule",
                newName: "VaccinationSchedule",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "ServiceHistory",
                newName: "ServiceHistory",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "PointTransaction",
                newName: "PointTransaction",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "PetPassport",
                newName: "PetPassport",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Pet",
                newName: "Pet",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payment",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "MicrochipItem",
                newName: "MicrochipItem",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Microchip",
                newName: "Microchip",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Membership",
                newName: "Membership",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "HealthCondition",
                newName: "HealthCondition",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Disease",
                newName: "Disease",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customer",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AppointmentDetail",
                newName: "AppointmentDetail",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Appointment",
                newName: "Appointment",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Account",
                newSchema: "public");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 12, 18, 7, 318, DateTimeKind.Utc).AddTicks(8369), "U/9U8WkN+7GYBqJl/8l0YA1SMqyiAGEb/+IyjkpJP7A=", "+yEw+hZ7HhIpLVy5NFlrAYVAryPOoKAeKI8ui5ETnKc=" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 12, 18, 7, 318, DateTimeKind.Utc).AddTicks(8375), "iqn9HI+IrCF5ETI8rwS/8T9hPyNWc2PzWC+n8f1mmtk=", "jhNWBSTYZSe3Sg7P7RLmbP7Jw9tlVm8pJQYtg8GdhBM=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "VetSchedule",
                schema: "public",
                newName: "VetSchedule");

            migrationBuilder.RenameTable(
                name: "Vet",
                schema: "public",
                newName: "Vet");

            migrationBuilder.RenameTable(
                name: "VaccineReceiptDetail",
                schema: "public",
                newName: "VaccineReceiptDetail");

            migrationBuilder.RenameTable(
                name: "VaccineReceipt",
                schema: "public",
                newName: "VaccineReceipt");

            migrationBuilder.RenameTable(
                name: "VaccineProfile",
                schema: "public",
                newName: "VaccineProfile");

            migrationBuilder.RenameTable(
                name: "VaccineExportDetail",
                schema: "public",
                newName: "VaccineExportDetail");

            migrationBuilder.RenameTable(
                name: "VaccineExport",
                schema: "public",
                newName: "VaccineExport");

            migrationBuilder.RenameTable(
                name: "VaccineDisease",
                schema: "public",
                newName: "VaccineDisease");

            migrationBuilder.RenameTable(
                name: "VaccineBatch",
                schema: "public",
                newName: "VaccineBatch");

            migrationBuilder.RenameTable(
                name: "Vaccine",
                schema: "public",
                newName: "Vaccine");

            migrationBuilder.RenameTable(
                name: "VaccinationSchedule",
                schema: "public",
                newName: "VaccinationSchedule");

            migrationBuilder.RenameTable(
                name: "ServiceHistory",
                schema: "public",
                newName: "ServiceHistory");

            migrationBuilder.RenameTable(
                name: "PointTransaction",
                schema: "public",
                newName: "PointTransaction");

            migrationBuilder.RenameTable(
                name: "PetPassport",
                schema: "public",
                newName: "PetPassport");

            migrationBuilder.RenameTable(
                name: "Pet",
                schema: "public",
                newName: "Pet");

            migrationBuilder.RenameTable(
                name: "Payment",
                schema: "public",
                newName: "Payment");

            migrationBuilder.RenameTable(
                name: "MicrochipItem",
                schema: "public",
                newName: "MicrochipItem");

            migrationBuilder.RenameTable(
                name: "Microchip",
                schema: "public",
                newName: "Microchip");

            migrationBuilder.RenameTable(
                name: "Membership",
                schema: "public",
                newName: "Membership");

            migrationBuilder.RenameTable(
                name: "HealthCondition",
                schema: "public",
                newName: "HealthCondition");

            migrationBuilder.RenameTable(
                name: "Disease",
                schema: "public",
                newName: "Disease");

            migrationBuilder.RenameTable(
                name: "Customer",
                schema: "public",
                newName: "Customer");

            migrationBuilder.RenameTable(
                name: "AppointmentDetail",
                schema: "public",
                newName: "AppointmentDetail");

            migrationBuilder.RenameTable(
                name: "Appointment",
                schema: "public",
                newName: "Appointment");

            migrationBuilder.RenameTable(
                name: "Account",
                schema: "public",
                newName: "Account");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 6, 16, 39, 916, DateTimeKind.Utc).AddTicks(7380), "BktpIzbXmzXznq5GvOlQBwQ0avGv8EGhjQQbbv44LCg=", "UpASQjK1TDKYtlYskR9mR6WS7M8Gk5PIliHAtIHnt6o=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 6, 16, 39, 916, DateTimeKind.Utc).AddTicks(7389), "Gl+MY/texaFJijNVSoRMJhPvDRix+ynsMeOOEaLtB3g=", "w98iEHWiC3MjkfCBTccj6m65TLZiWAeBKrokVtNlFZc=" });
        }
    }
}

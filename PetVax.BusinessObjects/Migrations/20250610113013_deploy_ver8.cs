using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class deploy_ver8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlaceOfBirth",
                schema: "dbo",
                table: "Pet",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlaceToLive",
                schema: "dbo",
                table: "Pet",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "VaccineProfileId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "VaccineBatchId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceType",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Reaction",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "PassportId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "NextVaccinationInfo",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "MicrochipItemId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "HealthConditionId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Dose",
                schema: "dbo",
                table: "AppointmentDetail",
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
                values: new object[] { new DateTime(2025, 6, 10, 11, 30, 12, 953, DateTimeKind.Utc).AddTicks(7962), "HRQefxZR6IgvtF4KmYvRmpAizJUHAuifxjumigmBDuo=", "hgX0RF3tA92KiPR+HSoLMu1i79BK7Wqve3mfDIILH8c=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 10, 11, 30, 12, 953, DateTimeKind.Utc).AddTicks(7966), "rY0F3fXkz6eBsh1KzY6kEfrj61mAYSbyuF7LVP0f9vU=", "LeyNPkrR9VQGaEtvdP2+2mpVzKLDkaX4xTR2YHu+9oI=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlaceOfBirth",
                schema: "dbo",
                table: "Pet");

            migrationBuilder.DropColumn(
                name: "PlaceToLive",
                schema: "dbo",
                table: "Pet");

            migrationBuilder.AlterColumn<int>(
                name: "VaccineProfileId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VaccineBatchId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ServiceType",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reaction",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PassportId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NextVaccinationInfo",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MicrochipItemId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HealthConditionId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Dose",
                schema: "dbo",
                table: "AppointmentDetail",
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
                values: new object[] { new DateTime(2025, 6, 7, 8, 58, 3, 854, DateTimeKind.Utc).AddTicks(5021), "frKBbXeFYdasVK9epBNUxOVwVHZoFbZNolBallw1Pm4=", "74o/6IC2bguzzMKzMZKd7w8+O2fv5+9P8vnysvJIQzY=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 8, 58, 3, 854, DateTimeKind.Utc).AddTicks(5027), "CXFSJXBznnCBsv8WTLZsPCBu/IBSdBHaxDkt/hsQE4s=", "TDiMo122I1y3aY2ObOWAE2WN1H70qhP51BkITeRmJY4=" });
        }
    }
}

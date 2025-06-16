using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class updatevp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDetail_VaccineProfile_VaccineProfileId",
                schema: "dbo",
                table: "AppointmentDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_VaccineProfile_Appointment_AppointmentId",
                schema: "dbo",
                table: "VaccineProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_VaccineProfile_VaccinationSchedule_VaccinationScheduleId1",
                schema: "dbo",
                table: "VaccineProfile");

            migrationBuilder.DropIndex(
                name: "IX_VaccineProfile_AppointmentId",
                schema: "dbo",
                table: "VaccineProfile");

            migrationBuilder.DropIndex(
                name: "IX_VaccineProfile_VaccinationScheduleId1",
                schema: "dbo",
                table: "VaccineProfile");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_VaccineProfileId",
                schema: "dbo",
                table: "AppointmentDetail");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                schema: "dbo",
                table: "VaccineProfile");

            migrationBuilder.DropColumn(
                name: "VaccinationScheduleId1",
                schema: "dbo",
                table: "VaccineProfile");

            migrationBuilder.DropColumn(
                name: "VaccineProfileId",
                schema: "dbo",
                table: "AppointmentDetail");

            migrationBuilder.RenameColumn(
                name: "VaccineScheduleId",
                schema: "dbo",
                table: "VaccineProfile",
                newName: "AppointmentDetailId");

            migrationBuilder.AlterColumn<int>(
                name: "VaccinationScheduleId",
                schema: "dbo",
                table: "VaccineProfile",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 16, 5, 37, 57, 883, DateTimeKind.Utc).AddTicks(5137), "Rb6g//sujEa8Zl5uYOZEqVuNKN3YKZLd20kHbErM4Pg=", "IC8cazfkiRRH+BsAj6YKKCTla9LKfQ7t0PIsqu03tN8=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 16, 5, 37, 57, 883, DateTimeKind.Utc).AddTicks(5142), "xNuy3n4I9++9xBjPKvk+vA+BBVeAv4ZHfR6/v/WJVAs=", "AGb7Ag0tiNCNoHzqe/XumDbxxbiXcVXyEZ1TCM8F7EQ=" });

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfile_AppointmentDetailId",
                schema: "dbo",
                table: "VaccineProfile",
                column: "AppointmentDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineProfile_AppointmentDetail_AppointmentDetailId",
                schema: "dbo",
                table: "VaccineProfile",
                column: "AppointmentDetailId",
                principalSchema: "dbo",
                principalTable: "AppointmentDetail",
                principalColumn: "AppointmentDetailId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccineProfile_AppointmentDetail_AppointmentDetailId",
                schema: "dbo",
                table: "VaccineProfile");

            migrationBuilder.DropIndex(
                name: "IX_VaccineProfile_AppointmentDetailId",
                schema: "dbo",
                table: "VaccineProfile");

            migrationBuilder.RenameColumn(
                name: "AppointmentDetailId",
                schema: "dbo",
                table: "VaccineProfile",
                newName: "VaccineScheduleId");

            migrationBuilder.AlterColumn<int>(
                name: "VaccinationScheduleId",
                schema: "dbo",
                table: "VaccineProfile",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                schema: "dbo",
                table: "VaccineProfile",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VaccinationScheduleId1",
                schema: "dbo",
                table: "VaccineProfile",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VaccineProfileId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 16, 5, 27, 27, 610, DateTimeKind.Utc).AddTicks(3926), "ZbSGrB/FlE6jBRUHClxtfcjg7qu6lmrjE2hi5cj4EeM=", "UCzZssBsB4Ydgw3qNvcq1pgW/uoojWXHV+3PyIZRQ9U=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 16, 5, 27, 27, 610, DateTimeKind.Utc).AddTicks(3930), "uGJgrdOcKQlDi+loF0K6y0PvNMcnRm26GAuyTHM86co=", "a/u+JjBIw0fqqSAgIkfAnD/nwVMf10GAmXZe0pszQwo=" });

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfile_AppointmentId",
                schema: "dbo",
                table: "VaccineProfile",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfile_VaccinationScheduleId1",
                schema: "dbo",
                table: "VaccineProfile",
                column: "VaccinationScheduleId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_VaccineProfileId",
                schema: "dbo",
                table: "AppointmentDetail",
                column: "VaccineProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDetail_VaccineProfile_VaccineProfileId",
                schema: "dbo",
                table: "AppointmentDetail",
                column: "VaccineProfileId",
                principalSchema: "dbo",
                principalTable: "VaccineProfile",
                principalColumn: "VaccineProfileId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineProfile_Appointment_AppointmentId",
                schema: "dbo",
                table: "VaccineProfile",
                column: "AppointmentId",
                principalSchema: "dbo",
                principalTable: "Appointment",
                principalColumn: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineProfile_VaccinationSchedule_VaccinationScheduleId1",
                schema: "dbo",
                table: "VaccineProfile",
                column: "VaccinationScheduleId1",
                principalSchema: "dbo",
                principalTable: "VaccinationSchedule",
                principalColumn: "VaccinationScheduleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

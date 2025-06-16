using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class updateVaccineProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "VaccineScheduleId",
                schema: "dbo",
                table: "VaccineProfile",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                schema: "dbo",
                table: "VaccineProfile");

            migrationBuilder.DropColumn(
                name: "VaccinationScheduleId1",
                schema: "dbo",
                table: "VaccineProfile");

            migrationBuilder.DropColumn(
                name: "VaccineScheduleId",
                schema: "dbo",
                table: "VaccineProfile");

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
                values: new object[] { new DateTime(2025, 6, 12, 13, 56, 58, 895, DateTimeKind.Utc).AddTicks(7601), "9Qbsyb1V7/w7XI4UlQDmWHAVr+/GJ/X0Q8oYsbhC2Bs=", "ZJ95VL6xis8Ln4rOtXnPQ/Ns0rpXwSOdPzYqiRSwqtI=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 12, 13, 56, 58, 895, DateTimeKind.Utc).AddTicks(7606), "9+F37WRPGyFNSaLMUfKXLh7IG2DQkYzNfQuxkZuy9wg=", "/3Ja7vzphpOujeZhz+uJlOU9OpdeLMaOQyZVb0UbY+w=" });
        }
    }
}

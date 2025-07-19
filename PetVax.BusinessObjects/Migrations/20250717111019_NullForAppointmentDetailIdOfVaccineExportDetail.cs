using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class NullForAppointmentDetailIdOfVaccineExportDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VaccineExportDetail_AppointmentDetailId",
                table: "VaccineExportDetail");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentDetailId",
                table: "VaccineExportDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 17, 11, 10, 18, 973, DateTimeKind.Utc).AddTicks(7109), "qNI4q/HYsKkPYdpkJtIjCuik+r3Afg/Gam/29egtpAg=", "lsiQInOBuI1ZHArIB7uJLhy+y8MhpFThIOMGhckgIp0=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 17, 11, 10, 18, 973, DateTimeKind.Utc).AddTicks(7118), "8g/0pIX+jppo4XBFwHXzrLF/Ac9vAPPhgOpPy0mQZh4=", "v4Ynga+vEyhyL99wFGvQ6JvIDTneJ3SkFit6h1S45SU=" });

            migrationBuilder.CreateIndex(
                name: "IX_VaccineExportDetail_AppointmentDetailId",
                table: "VaccineExportDetail",
                column: "AppointmentDetailId",
                unique: true,
                filter: "[AppointmentDetailId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VaccineExportDetail_AppointmentDetailId",
                table: "VaccineExportDetail");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentDetailId",
                table: "VaccineExportDetail",
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
                values: new object[] { new DateTime(2025, 7, 14, 14, 26, 40, 696, DateTimeKind.Utc).AddTicks(4425), "HV79d4eeb/WSvIIN+Xzp5Y/TkbTJhkqbgCB7cwi1eIs=", "L5KdKrw8JFT3JzlhZkJaquInwaECVmgedhVAUe7Vb6Q=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 14, 14, 26, 40, 696, DateTimeKind.Utc).AddTicks(4430), "NUHJv3/dGUP5Qt0vG8nLva/O4vbq/dDxs3CCNOjCEtA=", "BWEDxAJPo8FJBONt/tJJe3BOHNrYtG8BLeDlYiLR/OQ=" });

            migrationBuilder.CreateIndex(
                name: "IX_VaccineExportDetail_AppointmentDetailId",
                table: "VaccineExportDetail",
                column: "AppointmentDetailId",
                unique: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class updateDoseStringToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Dose",
                table: "VaccineProfile",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Dose",
                table: "AppointmentDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 25, 16, 0, 45, 495, DateTimeKind.Utc).AddTicks(7808), "P/MBXlC7CBtwL2Qrd+5OUfYYDIVE8awGWgFeeZYWFxc=", "/bbk5li4Tpzpz00xMaD3yoDZNNxxRFiy37QdzEIIwFE=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 25, 16, 0, 45, 495, DateTimeKind.Utc).AddTicks(7812), "DMuPx1kZ7ViCKQ8EhKdlwmBfnboXAj2HO+wmgKkEVic=", "dDSrn3AD2hoNFoBNE0n21vt+Rd80H6LKoLEPn9jfAyo=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Dose",
                table: "VaccineProfile",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Dose",
                table: "AppointmentDetail",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 24, 16, 42, 0, 937, DateTimeKind.Utc).AddTicks(8717), "JH8OviZ/aVyjUc5Xp80rs/RbXiKuoSCe7lrTZcT2rd0=", "tKpbaR3m6mLRh2lEES8ZK3AtA7o2FvZEN1F0ok+KHBU=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 24, 16, 42, 0, 937, DateTimeKind.Utc).AddTicks(8725), "h5gslHEPo8Ef25gmsnpY3ApHS85KdoZ31Tju1W6Oa/c=", "cacXCr7RsaTXWhpVi67QE4HKmQuX/Qv51RjXpN8AXBA=" });
        }
    }
}

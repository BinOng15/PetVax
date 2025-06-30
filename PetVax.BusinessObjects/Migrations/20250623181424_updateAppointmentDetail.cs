using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class updateAppointmentDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeneralCondition",
                table: "AppointmentDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeartRate",
                table: "AppointmentDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "AppointmentDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Others",
                table: "AppointmentDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temperature",
                table: "AppointmentDetail",
                type: "nvarchar(max)",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralCondition",
                table: "AppointmentDetail");

            migrationBuilder.DropColumn(
                name: "HeartRate",
                table: "AppointmentDetail");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "AppointmentDetail");

            migrationBuilder.DropColumn(
                name: "Others",
                table: "AppointmentDetail");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "AppointmentDetail");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 21, 10, 10, 49, 693, DateTimeKind.Utc).AddTicks(4957), "KKI8xAl1HG7Gy31OMdTlL02mjW7NXsDSAYxGvbaqHhg=", "iAf/a1wSceLpU/l4asEKNhysXO+zmvSzOsvXWDR/ccQ=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 21, 10, 10, 49, 693, DateTimeKind.Utc).AddTicks(4963), "a2LRW6W3fSUFF8Xob9XOctYK6237RjXO6M5a90TpmXo=", "sZoQyfJpNNy+0bZmpNNhXTLvhebZ6vI8JoXa+nK3rSk=" });
        }
    }
}

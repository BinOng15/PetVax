using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePayment123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Vaccine_VaccineId",
                table: "Payment");

            migrationBuilder.AddColumn<int>(
                name: "VaccineBatchId",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 17, 51, 44, 621, DateTimeKind.Utc).AddTicks(59), "AM+Cqg+T93Yd3EffVBL0W1NXV0z1KgUFLV/lcXkp444=", "xfEnO0qamPzhKZwjAfASnMG66h6rbTzKcgw2dfP6L04=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 17, 51, 44, 621, DateTimeKind.Utc).AddTicks(66), "EguLF/Cx1IAnwVqFTY8a/Qpku989sg1jUqelxAbVdp8=", "H4LcLV5liB5Nxd4Owq0w72edkofy/+AFVpUJmd7yGkE=" });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_VaccineBatchId",
                table: "Payment",
                column: "VaccineBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_VaccineBatch_VaccineBatchId",
                table: "Payment",
                column: "VaccineBatchId",
                principalTable: "VaccineBatch",
                principalColumn: "VaccineBatchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Vaccine_VaccineId",
                table: "Payment",
                column: "VaccineId",
                principalTable: "Vaccine",
                principalColumn: "VaccineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_VaccineBatch_VaccineBatchId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Vaccine_VaccineId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_VaccineBatchId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "VaccineBatchId",
                table: "Payment");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 16, 24, 11, 434, DateTimeKind.Utc).AddTicks(6240), "YFGKTXbBkCsYalsBeSLfMh/RBeH+SvhCLDVGXDv0nO4=", "DtnWoOHEUzgz4rqeEf5EMoEnqhlVDUsFRDq5XpOwk+E=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 3, 16, 24, 11, 434, DateTimeKind.Utc).AddTicks(6247), "tWNzrfWMhMnV5UF9dvlQ2DL379BVLuskV53CW6CzsO0=", "g9mLe0ThchGBZkCZ8eAO0CP82IV/W+c7t9/h88siKws=" });

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Vaccine_VaccineId",
                table: "Payment",
                column: "VaccineId",
                principalTable: "Vaccine",
                principalColumn: "VaccineId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

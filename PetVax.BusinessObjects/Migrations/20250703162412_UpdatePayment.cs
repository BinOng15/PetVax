using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<int>(
                name: "PaymentStatus",
                table: "Payment",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "HealthConditionId",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrochipId",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VaccinationCertificateId",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "HealthCondition",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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

            migrationBuilder.CreateIndex(
                name: "IX_Payment_HealthConditionId",
                table: "Payment",
                column: "HealthConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_MicrochipId",
                table: "Payment",
                column: "MicrochipId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_VaccinationCertificateId",
                table: "Payment",
                column: "VaccinationCertificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_HealthCondition_HealthConditionId",
                table: "Payment",
                column: "HealthConditionId",
                principalTable: "HealthCondition",
                principalColumn: "HealthConditionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Microchip_MicrochipId",
                table: "Payment",
                column: "MicrochipId",
                principalTable: "Microchip",
                principalColumn: "MicrochipId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_VaccinationCertificate_VaccinationCertificateId",
                table: "Payment",
                column: "VaccinationCertificateId",
                principalTable: "VaccinationCertificate",
                principalColumn: "CertificateId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_HealthCondition_HealthConditionId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Microchip_MicrochipId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_VaccinationCertificate_VaccinationCertificateId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_HealthConditionId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_MicrochipId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_VaccinationCertificateId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "HealthConditionId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "MicrochipId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "VaccinationCertificateId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "HealthCondition");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 2, 16, 40, 9, 517, DateTimeKind.Utc).AddTicks(9842), "rfmSbjG0/9bdjywjX9IQvosXiIYETkIY/LW8AZTjPLA=", "cwWXaJKNX+FpYuYESNpyfqW0rNi+QMxFLtj8bM/FYJg=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 2, 16, 40, 9, 517, DateTimeKind.Utc).AddTicks(9845), "Mq6Y7i6zpvcLmD1OZbiZwa81z4jeklUX5ZnGaVBdZPc=", "vFO3pf1SZVP5r7U/hHycbJeF4Rv5laYnRtjoI60vEnE=" });
        }
    }
}

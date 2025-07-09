using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class AddQrcodeFieldToPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 9, 14, 55, 27, 596, DateTimeKind.Utc).AddTicks(7388), "3gKB5Cr4jNrLVxuwFi8ip7DiJOyRAYbLO2ITa6DpzDI=", "IAlLrjGznYqX+gjB+at3s4DK5Im2M8FH1whtgS5b9es=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 9, 14, 55, 27, 596, DateTimeKind.Utc).AddTicks(7393), "gRpgMbXz1Jxo2+ZpamviZzy4ug1MxtHClG6HKoAM/Xg=", "rtlrMCUwX3U/0H2GR8Yg5f1GB2ZW9BRMN3a/VLlWSxk=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "Payment");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 9, 4, 41, 14, 100, DateTimeKind.Utc).AddTicks(4277), "461xgj9a3XXOKTDuR4pasLjSXLLP7B81RKbg7aq5Jik=", "cMGpAyNEsdIyjEzpR3dPUgXmtT6s2RW4RqegUoJwN9w=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 9, 4, 41, 14, 100, DateTimeKind.Utc).AddTicks(4283), "krJdH5J2y5v1M1dcB6U+2w4LryfdgwxjJwPG5dXt2QU=", "Gilp/txZRLPg3Qoj8pF7pWkJiya01HxUV6ILFNyeK7c=" });
        }
    }
}

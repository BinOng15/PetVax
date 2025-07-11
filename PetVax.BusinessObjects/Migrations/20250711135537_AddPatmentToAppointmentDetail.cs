using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class AddPatmentToAppointmentDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 11, 13, 55, 36, 900, DateTimeKind.Utc).AddTicks(7892), "QhVXvJTc14SwfrsYAt+d+prX2OEQM688ip8KCkpbVWA=", "KV6ozGA0yovXyqwEtPiYGLgZo3IFR8Lnz4+iEF9c+Ms=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 11, 13, 55, 36, 900, DateTimeKind.Utc).AddTicks(7905), "LIbUDoFd19JnkEm82EUySoawkcDQubSyJ3cLWNKDN4E=", "If5l1QUkI0rhUCWd7o/SJpKXe+SLCQq2Deor0aBiM5o=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}

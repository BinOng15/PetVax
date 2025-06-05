using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class postgre_version2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 5, 6, 26, 18, 667, DateTimeKind.Utc).AddTicks(7466), "BP9BYTbpwX75d2UEot006A==", "879YbsTO6kK8QcG1RkHHzQ==" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 5, 6, 26, 18, 667, DateTimeKind.Utc).AddTicks(7471), "bxuFGMUiJjDiAlRexof4CQ==", "R02g4LHg3f6h3zvWpH1Tig==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 5, 5, 36, 50, 506, DateTimeKind.Utc).AddTicks(9619), "v5rDRaM7z7JhDE0gWTCyHA==", "AT2esuw7PY7jGu1cK058NQ==" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 5, 5, 36, 50, 506, DateTimeKind.Utc).AddTicks(9623), "iWtdnFsfg1n/5S5RlztyhA==", "AT2esuw7PY7jGu1cK058NQ==" });
        }
    }
}

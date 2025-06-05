using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class dtvh : Migration
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
                values: new object[] { new DateTime(2025, 6, 5, 7, 29, 13, 726, DateTimeKind.Utc).AddTicks(273), "jYMxb+56V+NdWnjzAxdrHA==", "CendnSaEfV2c9GsJhIW4IQ==" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 5, 7, 29, 13, 726, DateTimeKind.Utc).AddTicks(283), "ehOL60U1rhvWNfu0u2qe5Q==", "R3S3GaYUV6WW9bxw3xcqMg==" });
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
                values: new object[] { new DateTime(2025, 6, 5, 6, 26, 18, 667, DateTimeKind.Utc).AddTicks(7466), "BP9BYTbpwX75d2UEot006A==", "879YbsTO6kK8QcG1RkHHzQ==" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 5, 6, 26, 18, 667, DateTimeKind.Utc).AddTicks(7471), "bxuFGMUiJjDiAlRexof4CQ==", "R02g4LHg3f6h3zvWpH1Tig==" });
        }
    }
}

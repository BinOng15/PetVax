using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class ver5 : Migration
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
                values: new object[] { new DateTime(2025, 6, 5, 7, 48, 56, 301, DateTimeKind.Utc).AddTicks(2322), "F3paenpsVK+JcK16nL20HYQ6ecIxzd3iWReyEqCNOds=", "DMGagyXqsBxkvaqdA2GygFXKu0aFPcVCuwyX3+5f7VI=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 5, 7, 48, 56, 301, DateTimeKind.Utc).AddTicks(2329), "bNDfE9sHBCrxyBUdxdAeeq7V9C3EqTQFuSRedOVTy6M=", "1ojf/bBP+TFBOac6IQgJiECzQ7eXkR1Nv5YlHZiYjFg=" });
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
                values: new object[] { new DateTime(2025, 6, 5, 7, 29, 13, 726, DateTimeKind.Utc).AddTicks(273), "jYMxb+56V+NdWnjzAxdrHA==", "CendnSaEfV2c9GsJhIW4IQ==" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 5, 7, 29, 13, 726, DateTimeKind.Utc).AddTicks(283), "ehOL60U1rhvWNfu0u2qe5Q==", "R3S3GaYUV6WW9bxw3xcqMg==" });
        }
    }
}

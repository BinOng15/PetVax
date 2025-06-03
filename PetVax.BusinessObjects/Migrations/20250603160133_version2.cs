using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class version2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 6, 3, 16, 1, 33, 376, DateTimeKind.Utc).AddTicks(1734), "x1RE/1IQUsTz+mCzesN8aax1DbRPWA4dTqqPlKkyqSY=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 6, 3, 16, 1, 33, 376, DateTimeKind.Utc).AddTicks(1738), "5F0TFOP3iaFynnG3+upjmYD3PKe9oHS0MAVEXKECpuc=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 6, 3, 15, 25, 0, 584, DateTimeKind.Utc).AddTicks(517), "tns+8qNsHzTeEu/NbYni0t43PI5KMl0gXbEEN8qjMoQ=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 6, 3, 15, 25, 0, 584, DateTimeKind.Utc).AddTicks(522), "gYMd4KGBD2sEeUuepPrVJ6INHXlkOWjBzvJvU7rRON0=" });
        }
    }
}

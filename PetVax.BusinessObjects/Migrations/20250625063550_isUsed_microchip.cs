using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class isUsed_microchip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "MicrochipItem",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 25, 6, 35, 50, 157, DateTimeKind.Utc).AddTicks(9291), "a66PWWq3O3Ow+YsiBAtqjEZLbMcHNg/jVoAR1u72pKk=", "av8ErR+4xoBsPPSul1JbeoY8QLiTNIce/Z7HlpRbw3o=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 25, 6, 35, 50, 157, DateTimeKind.Utc).AddTicks(9296), "pkJLo00KOUGF9Rq9zKdi3Do/K7B5wML5bWtUyqL/v70=", "E8SCuUy3q4BDikDaSYDBVGn2WFjCih7u7iELPZN4ySM=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "MicrochipItem");

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
    }
}

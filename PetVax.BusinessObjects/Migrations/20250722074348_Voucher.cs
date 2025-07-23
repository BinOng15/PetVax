using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class Voucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CurrentPoints",
                table: "Customer",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSpent",
                table: "Customer",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    VoucherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    VoucherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.VoucherId);
                    table.ForeignKey(
                        name: "FK_Voucher_PointTransaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "PointTransaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 22, 7, 43, 47, 258, DateTimeKind.Utc).AddTicks(3965), "AqD7kRxgZafHFUIDEB9MCAJHS4/p/+lAr9W59d9uzWw=", "gBLJMX/zHFnePiEOLwMqeJlYXeDtrcHptCbxtsu61kY=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 22, 7, 43, 47, 258, DateTimeKind.Utc).AddTicks(3978), "cTSyKpm9HW8P/aDzd+vUMBLMoKJw0hLMJvoQNXZrDXU=", "Rsr2JOrtV2fjsID5GWN9YJ6X4N3Hm+IEiAXCaR0AHjU=" });

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_TransactionId",
                table: "Voucher",
                column: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropColumn(
                name: "TotalSpent",
                table: "Customer");

            migrationBuilder.AlterColumn<string>(
                name: "CurrentPoints",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 17, 11, 10, 18, 973, DateTimeKind.Utc).AddTicks(7109), "qNI4q/HYsKkPYdpkJtIjCuik+r3Afg/Gam/29egtpAg=", "lsiQInOBuI1ZHArIB7uJLhy+y8MhpFThIOMGhckgIp0=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 17, 11, 10, 18, 973, DateTimeKind.Utc).AddTicks(7118), "8g/0pIX+jppo4XBFwHXzrLF/Ac9vAPPhgOpPy0mQZh4=", "v4Ynga+vEyhyL99wFGvQ6JvIDTneJ3SkFit6h1S45SU=" });
        }
    }
}

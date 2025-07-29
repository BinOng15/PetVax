using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePointTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ServiceType",
                table: "ServiceHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "PointTransaction",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoucherId",
                table: "PointTransaction",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 27, 16, 5, 23, 374, DateTimeKind.Utc).AddTicks(8485), "djYJdXtkHUI89pvit8YGwQfamXqeUrzQnbxz7Zfyq60=", "KxU7SS0fp9/JTLt1inEcRZmKdfs9nHOS5QRp/MNMC+s=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 27, 16, 5, 23, 374, DateTimeKind.Utc).AddTicks(8497), "lE+K91fxLReO2DjRJntsPOFnTTu5w0AeHsF6rGfFJ2s=", "QBq6zHCeir6Xy5YzjDPEs7zcYKFLmKs4VsP5Vp2zLhI=" });

            migrationBuilder.CreateIndex(
                name: "IX_PointTransaction_PaymentId",
                table: "PointTransaction",
                column: "PaymentId",
                unique: true,
                filter: "[PaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransaction_VoucherId",
                table: "PointTransaction",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointTransaction_Payment_PaymentId",
                table: "PointTransaction",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "PaymentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PointTransaction_Voucher_VoucherId",
                table: "PointTransaction",
                column: "VoucherId",
                principalTable: "Voucher",
                principalColumn: "VoucherId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointTransaction_Payment_PaymentId",
                table: "PointTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTransaction_Voucher_VoucherId",
                table: "PointTransaction");

            migrationBuilder.DropIndex(
                name: "IX_PointTransaction_PaymentId",
                table: "PointTransaction");

            migrationBuilder.DropIndex(
                name: "IX_PointTransaction_VoucherId",
                table: "PointTransaction");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "PointTransaction");

            migrationBuilder.DropColumn(
                name: "VoucherId",
                table: "PointTransaction");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceType",
                table: "ServiceHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 25, 3, 58, 13, 745, DateTimeKind.Utc).AddTicks(9186), "IfzaJGP4ouUBkY/kh6Fa9dctuwvn0KtzbRbnHWpf8rA=", "1zFe4TPiAN4N6+0NOt30VQR3HL9lUse5oR/REkA2prs=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 25, 3, 58, 13, 745, DateTimeKind.Utc).AddTicks(9190), "dBGl1mCaFYxe42Qb88gaHc3LBSphaj9CgF5JamgMviM=", "yFa4tpK1vH354RDHlAyd8uaS4ziPbfVZNVYKcyYbsr4=" });
        }
    }
}

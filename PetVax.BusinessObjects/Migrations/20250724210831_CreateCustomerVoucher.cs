using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class CreateCustomerVoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerVoucher",
                columns: table => new
                {
                    CustomerVoucherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    VoucherId = table.Column<int>(type: "int", nullable: false),
                    RedeemedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RedeemedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerVoucher", x => x.CustomerVoucherId);
                    table.ForeignKey(
                        name: "FK_CustomerVoucher_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerVoucher_Voucher_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Voucher",
                        principalColumn: "VoucherId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 21, 8, 30, 579, DateTimeKind.Utc).AddTicks(7820), "Sai24HCjZnOKw2KjVHIva3UXXV4QkXIyjhbDV5u20Hg=", "cSFsUgUlmPcaoEvM8EMBKoIYaKMyfv5KIr8CxBLUf4Q=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 21, 8, 30, 579, DateTimeKind.Utc).AddTicks(7831), "WPL+KyOFqQ/VKuP8rT2mYgGac3x8ApNYgQWCHuaGuEg=", "NTseK+ye6f2KYWE2aJKXoNjGFQsX5R5Di+7dSEZx6nY=" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerVoucher_CustomerId",
                table: "CustomerVoucher",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerVoucher_VoucherId",
                table: "CustomerVoucher",
                column: "VoucherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerVoucher");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 20, 26, 35, 312, DateTimeKind.Utc).AddTicks(3904), "kUICsl0yTstH4xK7rNjm763zbJeWJ9OOLAFK46xRabU=", "8kgGC2mD6lIon63ZzoU30o09adiVYN2kjxNwdorNVkQ=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 20, 26, 35, 312, DateTimeKind.Utc).AddTicks(3911), "9Mf0CUM3jos+SAnn3xSB4yIURzMAMwC0bXyvg9SK4Zc=", "07gIE1bKnX83rr8snK4TbhZZ+oXMrBhuiZYjVvePdBE=" });
        }
    }
}

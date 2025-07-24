using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class AddNullForTransactionIdInVoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "Voucher",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "Voucher",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 15, 59, 53, 721, DateTimeKind.Utc).AddTicks(8006), "JF0GVOF6O/fkBzQR1mfxk9GDEkgtKHYqJeWqBlfEp+s=", "DZ0xbFMRfgEUDxclXefm3pLBuIscESg3ASH0P0yBZ9c=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 15, 59, 53, 721, DateTimeKind.Utc).AddTicks(8010), "Cz+LAhMCrYofzHNYgR1HW21DC5f4+vapn9aLSpDlo3U=", "TOhJThrmGfiP60S3yWbZndNiwfIfHi6nKJTr8QXiw9U=" });
        }
    }
}

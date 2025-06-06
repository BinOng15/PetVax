using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class postgre_ver9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                schema: "dbo",
                table: "Customer",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 6, 8, 23, 4, 796, DateTimeKind.Utc).AddTicks(7705), "womvxz/kDHfCRHHPwYteuwiP3CWoWSuF7hdDwaYJqBU=", "Y9yMaaADCI09IHa5G2FDgck/WeUCpGZkkORTXziyOzc=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 6, 8, 23, 4, 796, DateTimeKind.Utc).AddTicks(7709), "H6r/MEEyn8wCsTiM9AXV/5IOB0jjONSi1zAnboeaeJc=", "8g2cGHLm86Sfvtvm0SwbtY3RJ4fmsNx32TiULIwPOmk=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                schema: "dbo",
                table: "Customer",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 6, 8, 21, 8, 299, DateTimeKind.Utc).AddTicks(2870), "qcl5XBTwGQJfnWKO8IHbvUpFRY0k9hE0aB432IGLO+k=", "neoOsY7GqEJjNW2j1devJ1An7YK9aTcUZTT6enB16TM=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 6, 8, 21, 8, 299, DateTimeKind.Utc).AddTicks(2875), "PAqtm4O6Nm7uCNSp6a8ypqTNqKmClrp0SNd0p0bVmXI=", "UJFA8SFupPp0kTrQtqfco7vxt48hL/OLq8vvBqTkpqI=" });
        }
    }
}

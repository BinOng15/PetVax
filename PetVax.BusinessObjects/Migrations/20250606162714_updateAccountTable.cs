using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class updateAccountTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordSalt",
                schema: "dbo",
                table: "Account",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                schema: "dbo",
                table: "Account",
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
                values: new object[] { new DateTime(2025, 6, 6, 16, 27, 13, 774, DateTimeKind.Utc).AddTicks(9096), "3/grvM501nv9q0SdviLbKzAyozK7TC7eiUPlijsf4HM=", "/i0zay2uP96I3g3GyMs8mBUI7zfPQ+VVH0MvDoLYrCM=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 6, 16, 27, 13, 774, DateTimeKind.Utc).AddTicks(9104), "S/BDGGmNUzmHqucOuUYZpVlRn4d1k7i92jlIuuJbNOM=", "+YOenQ/oiFrLhADqwTyhovdvF5xV3O/6E7D3IXP3Ke8=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordSalt",
                schema: "dbo",
                table: "Account",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                schema: "dbo",
                table: "Account",
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
                values: new object[] { new DateTime(2025, 6, 6, 15, 25, 16, 639, DateTimeKind.Utc).AddTicks(6165), "yxtj3f2xcKbwnOR0JS2LIppMqup76kL4Tfbgg8PocjI=", "Sd1oZef4J8GW7nihvkj0b86B+0857HHqxCHZwAwVYmk=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 6, 15, 25, 16, 639, DateTimeKind.Utc).AddTicks(6173), "QS85E9Fn13EJpcIn6RV5dPaNnRLX8QZ3YCiBMhDuVK8=", "3d1JwtKlYjwHIbSs1LBO+rX1c8ZadZI+VYdpls3Iaek=" });
        }
    }
}

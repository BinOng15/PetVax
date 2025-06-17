using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVer13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                schema: "dbo",
                table: "Customer",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VetId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 17, 11, 2, 16, 854, DateTimeKind.Utc).AddTicks(1977), "UNVWxAtt+E1QI04q+XTUBxJBSpiwyfkbud78z5g2BXQ=", "BqiyeMxTP7aIgdSVoHZHqWtIq5SKRlWTaDk4j1f2JtQ=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 17, 11, 2, 16, 854, DateTimeKind.Utc).AddTicks(1982), "qc2xxr93Kos1zyQ0cZuwXwhu5E+VQLwypeqkQdBsonQ=", "exvfw/X+lxFkrK16DAaqeKtvRtWw4i6kwDjoZ2ATY5M=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.AlterColumn<int>(
                name: "VetId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 17, 9, 9, 2, 265, DateTimeKind.Utc).AddTicks(9611), "aYoOFGIrwzJur8jU2/g956N+eByL0PtPrNs8ZqFWNmo=", "wTYlwBtruLSWXs/GY9KLiOso+qjMv2W/OfUZwmwv0xM=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 17, 9, 9, 2, 265, DateTimeKind.Utc).AddTicks(9619), "ja8Pm6QEzLZCN7dKOUqDKJJ/5IRZQqs5zqSzHy2J9Gs=", "eJUmbYh4vvy1ZJmnpyG1I1B8J6ySojHONPejN9MNfYQ=" });
        }
    }
}

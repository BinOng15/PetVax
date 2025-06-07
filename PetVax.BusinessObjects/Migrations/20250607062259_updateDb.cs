using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class updateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isVerify",
                schema: "dbo",
                table: "Account",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "isVerify" },
                values: new object[] { new DateTime(2025, 6, 7, 6, 22, 58, 556, DateTimeKind.Utc).AddTicks(972), "ygQR/N8hIxdcpLqI78bdTkLG+FxjmcaYzFNUjI9+vAI=", "f42aNYfmO67SYEIa/JxHayZXeqCbJHY+yxOIbtTpJl4=", true });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "isVerify" },
                values: new object[] { new DateTime(2025, 6, 7, 6, 22, 58, 556, DateTimeKind.Utc).AddTicks(980), "Mc7HHG2XwLec2ra0jgqRw9AsE+y/T+XABDp0y9y/GYk=", "ysYV7I+8LcrfUeS7NKL9kLoA8oJN7FlXRMdLsZVojMQ=", true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isVerify",
                schema: "dbo",
                table: "Account");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 4, 17, 38, 439, DateTimeKind.Utc).AddTicks(3041), "QMRt8ScCnXvAaU7JLv5qNE+9z4cuIkCt6YHfnDc/IIY=", "B2jbJmbc++CnKrN9FVNoSBDwNHnS5+uzdowdgj2tKZc=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 4, 17, 38, 439, DateTimeKind.Utc).AddTicks(3048), "e+PoAH47EFa6Q8YvHaWNdR2xcH7/MSPOO68uUa82mGE=", "FxG1F5DBodja3WC0CZwEOQsEzyTQP3/mDKgdOSE9zVA=" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class update5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 3, 56, 7, 129, DateTimeKind.Utc).AddTicks(3875), "328rpjNqPg15gzybzsZQ4W9l/ia0pheywGChBkgjq/I=", "dMYKAMT5WuyifUiUo5amUmd+HhbI5/yGI2o8h9jTCgw=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 3, 56, 7, 129, DateTimeKind.Utc).AddTicks(3879), "W81JHeh7VLq2bWhGJTA3p06TurVWajL3k6liZzPiTeY=", "ix9Rxs1ZP1NZ67Ua/vra/nsExZxiXM/Q9uOYL8qPiYU=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 6, 18, 53, 34, 834, DateTimeKind.Utc).AddTicks(9349), "9BPVobbIcXWO4VI9X8q8ZQpLyFFhKigXJKKYn2e9L1s=", "ChnfhkJx3fu+r2FNTdOsz5vaCiz0/cC7IUbAnUknV/Y=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 6, 18, 53, 34, 834, DateTimeKind.Utc).AddTicks(9354), "khJDtR17Ug/dIUHxfGzBwOsNj9CpBERN/zKLHbhIW6M=", "YJdQ94Uzwu3pj+jdfLxUN7y6jG+ONbc6kSGK35c4pAE=" });
        }
    }
}

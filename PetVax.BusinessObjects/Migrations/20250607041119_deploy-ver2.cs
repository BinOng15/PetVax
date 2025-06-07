using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class deployver2 : Migration
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
                values: new object[] { new DateTime(2025, 6, 7, 4, 11, 18, 571, DateTimeKind.Utc).AddTicks(6079), "X0Q9mGaJuJNPWOY5Zs59sRZqTJw8eakk1BpOi7YlMGY=", "YJibkwEW9gz4B9dCzVOUL7uiTy/Eu/jhUH0Zi8KDORk=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 4, 11, 18, 571, DateTimeKind.Utc).AddTicks(6085), "pKnovavaFHmStk7aI6Zfh6AW1FS0jMS4LdXcCt/sIJ8=", "NDSXymqqmqSdTBs0IT2MQoId0nYB7EWkZhsouTSgIGE=" });
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
                values: new object[] { new DateTime(2025, 6, 7, 3, 56, 7, 129, DateTimeKind.Utc).AddTicks(3875), "328rpjNqPg15gzybzsZQ4W9l/ia0pheywGChBkgjq/I=", "dMYKAMT5WuyifUiUo5amUmd+HhbI5/yGI2o8h9jTCgw=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 3, 56, 7, 129, DateTimeKind.Utc).AddTicks(3879), "W81JHeh7VLq2bWhGJTA3p06TurVWajL3k6liZzPiTeY=", "ix9Rxs1ZP1NZ67Ua/vra/nsExZxiXM/Q9uOYL8qPiYU=" });
        }
    }
}

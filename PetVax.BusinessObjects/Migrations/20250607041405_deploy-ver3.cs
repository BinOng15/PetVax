using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class deployver3 : Migration
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
                values: new object[] { new DateTime(2025, 6, 7, 4, 14, 4, 682, DateTimeKind.Utc).AddTicks(5738), "+2J1dugZ0Dups9Y/aP06rLEt3nLFKJnYl8GjzH+97vY=", "jUI4XsK3J0RZFkbF4aND9zyqh1zmTjJzqkh48WuA6ag=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 4, 14, 4, 682, DateTimeKind.Utc).AddTicks(5741), "29BY7Et2vNL8N2mqXdg1GGNfzlnb+81w7U2WmzGEGoM=", "Zve20x8V28J/EGCHU48DqLMNGe6cuUd4eYFvO88HkmI=" });
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
                values: new object[] { new DateTime(2025, 6, 7, 4, 11, 18, 571, DateTimeKind.Utc).AddTicks(6079), "X0Q9mGaJuJNPWOY5Zs59sRZqTJw8eakk1BpOi7YlMGY=", "YJibkwEW9gz4B9dCzVOUL7uiTy/Eu/jhUH0Zi8KDORk=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 4, 11, 18, 571, DateTimeKind.Utc).AddTicks(6085), "pKnovavaFHmStk7aI6Zfh6AW1FS0jMS4LdXcCt/sIJ8=", "NDSXymqqmqSdTBs0IT2MQoId0nYB7EWkZhsouTSgIGE=" });
        }
    }
}

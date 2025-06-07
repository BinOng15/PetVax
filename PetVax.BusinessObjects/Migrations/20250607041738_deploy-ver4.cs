using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class deployver4 : Migration
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
                values: new object[] { new DateTime(2025, 6, 7, 4, 17, 38, 439, DateTimeKind.Utc).AddTicks(3041), "QMRt8ScCnXvAaU7JLv5qNE+9z4cuIkCt6YHfnDc/IIY=", "B2jbJmbc++CnKrN9FVNoSBDwNHnS5+uzdowdgj2tKZc=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 4, 17, 38, 439, DateTimeKind.Utc).AddTicks(3048), "e+PoAH47EFa6Q8YvHaWNdR2xcH7/MSPOO68uUa82mGE=", "FxG1F5DBodja3WC0CZwEOQsEzyTQP3/mDKgdOSE9zVA=" });
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
                values: new object[] { new DateTime(2025, 6, 7, 4, 14, 4, 682, DateTimeKind.Utc).AddTicks(5738), "+2J1dugZ0Dups9Y/aP06rLEt3nLFKJnYl8GjzH+97vY=", "jUI4XsK3J0RZFkbF4aND9zyqh1zmTjJzqkh48WuA6ag=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 4, 14, 4, 682, DateTimeKind.Utc).AddTicks(5741), "29BY7Et2vNL8N2mqXdg1GGNfzlnb+81w7U2WmzGEGoM=", "Zve20x8V28J/EGCHU48DqLMNGe6cuUd4eYFvO88HkmI=" });
        }
    }
}

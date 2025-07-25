using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class update_databaseServiceHistory2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 25, 3, 58, 13, 745, DateTimeKind.Utc).AddTicks(9186), "IfzaJGP4ouUBkY/kh6Fa9dctuwvn0KtzbRbnHWpf8rA=", "1zFe4TPiAN4N6+0NOt30VQR3HL9lUse5oR/REkA2prs=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 25, 3, 58, 13, 745, DateTimeKind.Utc).AddTicks(9190), "dBGl1mCaFYxe42Qb88gaHc3LBSphaj9CgF5JamgMviM=", "yFa4tpK1vH354RDHlAyd8uaS4ziPbfVZNVYKcyYbsr4=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 25, 3, 6, 57, 642, DateTimeKind.Utc).AddTicks(1748), "cztZD1B1MXEB8ZZieaQCRZNcvWsDUJ3AnCyS3tOhuFM=", "7Alw0w8dXgZPeBKUzKxXW4iIu8uUcDQt/K0QH1nrfXs=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 25, 3, 6, 57, 642, DateTimeKind.Utc).AddTicks(1752), "nbOV4t43aBkimhtaScav3UAvnMDtzHtcUI+KjHLKKCA=", "omRCvnDC9EEDhqUOlyFf2vl/kS7eVsKqGjiWTdOb0QE=" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class DeployVer2 : Migration
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
                values: new object[] { new DateTime(2025, 6, 17, 15, 36, 45, 985, DateTimeKind.Utc).AddTicks(2732), "pT7nTY1B1sTK9uG6hFk0GSyGAx7zpY1GD5qEMZSg2h0=", "a+6nfn79B7Bns0rqmQ7JJQBTYa5Lyk+nBV3UcUXsDH4=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 17, 15, 36, 45, 985, DateTimeKind.Utc).AddTicks(2737), "OAbDfIckamF/Ew7wpD42k0y7cUnsVE9iRtXX9DGqXo8=", "EfHo6C1rrYubJxyx6h+1TesIbramCp1qyAFQSsPynGY=" });
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
                values: new object[] { new DateTime(2025, 6, 17, 11, 2, 16, 854, DateTimeKind.Utc).AddTicks(1977), "UNVWxAtt+E1QI04q+XTUBxJBSpiwyfkbud78z5g2BXQ=", "BqiyeMxTP7aIgdSVoHZHqWtIq5SKRlWTaDk4j1f2JtQ=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 17, 11, 2, 16, 854, DateTimeKind.Utc).AddTicks(1982), "qc2xxr93Kos1zyQ0cZuwXwhu5E+VQLwypeqkQdBsonQ=", "exvfw/X+lxFkrK16DAaqeKtvRtWw4i6kwDjoZ2ATY5M=" });
        }
    }
}

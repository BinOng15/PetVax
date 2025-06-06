using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class newSeedData : Migration
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
                values: new object[] { new DateTime(2025, 6, 6, 15, 25, 16, 639, DateTimeKind.Utc).AddTicks(6165), "yxtj3f2xcKbwnOR0JS2LIppMqup76kL4Tfbgg8PocjI=", "Sd1oZef4J8GW7nihvkj0b86B+0857HHqxCHZwAwVYmk=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 6, 15, 25, 16, 639, DateTimeKind.Utc).AddTicks(6173), "QS85E9Fn13EJpcIn6RV5dPaNnRLX8QZ3YCiBMhDuVK8=", "3d1JwtKlYjwHIbSs1LBO+rX1c8ZadZI+VYdpls3Iaek=" });
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
                values: new object[] { new DateTime(2025, 6, 6, 8, 23, 4, 796, DateTimeKind.Utc).AddTicks(7705), "womvxz/kDHfCRHHPwYteuwiP3CWoWSuF7hdDwaYJqBU=", "Y9yMaaADCI09IHa5G2FDgck/WeUCpGZkkORTXziyOzc=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 6, 8, 23, 4, 796, DateTimeKind.Utc).AddTicks(7709), "H6r/MEEyn8wCsTiM9AXV/5IOB0jjONSi1zAnboeaeJc=", "8g2cGHLm86Sfvtvm0SwbtY3RJ4fmsNx32TiULIwPOmk=" });
        }
    }
}

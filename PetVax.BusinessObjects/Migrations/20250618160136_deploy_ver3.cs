using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class deploy_ver3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 16, 1, 35, 777, DateTimeKind.Utc).AddTicks(913), "jg0WEgGekaJlB2EG58I32ifHB1vNfYDGKb/JoP6sWOs=", "s4Fc/lVm0VAB7eKQy2LJLzxdpzV0Gq8JyT03ru9W2zw=" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 16, 1, 35, 777, DateTimeKind.Utc).AddTicks(920), "Xuk/8acYBGwarnaz+vL5f6q4nPw/pzoKX5wurVaRnJM=", "NFX094gZVlRf4FeUOEuaBExEqSzJrEl3swxkH92PqBY=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 15, 57, 36, 73, DateTimeKind.Utc).AddTicks(88), "zFAJUjhTUQ1m3WutsVuKsgcFO4cBnoyha3BNtFdN3B0=", "fkHkj8MxLIcn7epqkB+HmSuownQwmxU7W+aYjhTnpEU=" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 15, 57, 36, 73, DateTimeKind.Utc).AddTicks(92), "0xIJ/2zlbisNDttDGXBFOSRk8msYcvB9U1rMUsZSuao=", "DbDiLmpcvRgF+YcLRj2ej8er+6UA57HRqqiDAyHFyK8=" });
        }
    }
}

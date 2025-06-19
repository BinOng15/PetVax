using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class deploy_ver4 : Migration
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
                values: new object[] { new DateTime(2025, 6, 18, 16, 1, 59, 212, DateTimeKind.Utc).AddTicks(2039), "o4CEClMUlDktFO+7P5yim8DuaAan2ykfVoQcYB1f9V8=", "JcB/CpaEdUAWMDS1Cpe8R+ZOQhb1Hutw6h7o5lenR+Y=" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 16, 1, 59, 212, DateTimeKind.Utc).AddTicks(2046), "QjqF33sw8CJ5U7GSAVRqO40yUpBtr1WOCKE7Db/TZXY=", "bg4WDf06M3pXYit/fqsTsRYn1ZlPNoapMkkxKW7EKKE=" });
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
                values: new object[] { new DateTime(2025, 6, 18, 16, 1, 35, 777, DateTimeKind.Utc).AddTicks(913), "jg0WEgGekaJlB2EG58I32ifHB1vNfYDGKb/JoP6sWOs=", "s4Fc/lVm0VAB7eKQy2LJLzxdpzV0Gq8JyT03ru9W2zw=" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 16, 1, 35, 777, DateTimeKind.Utc).AddTicks(920), "Xuk/8acYBGwarnaz+vL5f6q4nPw/pzoKX5wurVaRnJM=", "NFX094gZVlRf4FeUOEuaBExEqSzJrEl3swxkH92PqBY=" });
        }
    }
}

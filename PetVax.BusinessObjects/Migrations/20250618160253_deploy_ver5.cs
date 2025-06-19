using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class deploy_ver5 : Migration
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
                values: new object[] { new DateTime(2025, 6, 18, 16, 2, 53, 180, DateTimeKind.Utc).AddTicks(8126), "Mjno4B507f+5gyc9kXFwMd3xhgiBeo/ENKTcRz1x1Co=", "jmv+HwVVg5kNNq6svnp2qGml17CaH56FertfPMDxHmI=" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 16, 2, 53, 180, DateTimeKind.Utc).AddTicks(8131), "jABiHgaGfhVnAqe2d0AtpZeYDbklUbasHmkVZ5hE3+A=", "4yVipqODBS0hScnZLFUfnQUFHm4UANVxcnaQF1BYmj8=" });
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
                values: new object[] { new DateTime(2025, 6, 18, 16, 1, 59, 212, DateTimeKind.Utc).AddTicks(2039), "o4CEClMUlDktFO+7P5yim8DuaAan2ykfVoQcYB1f9V8=", "JcB/CpaEdUAWMDS1Cpe8R+ZOQhb1Hutw6h7o5lenR+Y=" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 16, 1, 59, 212, DateTimeKind.Utc).AddTicks(2046), "QjqF33sw8CJ5U7GSAVRqO40yUpBtr1WOCKE7Db/TZXY=", "bg4WDf06M3pXYit/fqsTsRYn1ZlPNoapMkkxKW7EKKE=" });
        }
    }
}

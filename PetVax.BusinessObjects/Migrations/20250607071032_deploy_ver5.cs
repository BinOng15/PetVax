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
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 7, 10, 31, 812, DateTimeKind.Utc).AddTicks(3233), "5po8BBg3Q5qdF4mMe0aFUQACOqvOVobDj6e5gQI0Fss=", "69/fo+nWWao8whHgeY2Pf3VE5kA4mCs8QXUEDWalXqk=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 7, 10, 31, 812, DateTimeKind.Utc).AddTicks(3236), "s65t4E95xadQPguq0ivzTuqMa3QCoFY8rbkQ8yNRbzE=", "hDCZtV+rGmq5ovI05AXdlbnRj7vpZveXgoLWAmrxwlY=" });
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
                values: new object[] { new DateTime(2025, 6, 7, 6, 22, 58, 556, DateTimeKind.Utc).AddTicks(972), "ygQR/N8hIxdcpLqI78bdTkLG+FxjmcaYzFNUjI9+vAI=", "f42aNYfmO67SYEIa/JxHayZXeqCbJHY+yxOIbtTpJl4=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 7, 6, 22, 58, 556, DateTimeKind.Utc).AddTicks(980), "Mc7HHG2XwLec2ra0jgqRw9AsE+y/T+XABDp0y9y/GYk=", "ysYV7I+8LcrfUeS7NKL9kLoA8oJN7FlXRMdLsZVojMQ=" });
        }
    }
}

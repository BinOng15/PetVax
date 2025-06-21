using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class microchipItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 20, 8, 11, 7, 526, DateTimeKind.Utc).AddTicks(7155), "xNGjSmw/QmlUDG6WM468ZNHfYbWDnbmJmwa8+feO0zk=", "VccD2czsuEicBDB59SI7w/gnSGxcATupGzVOg7lwab4=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 20, 8, 11, 7, 526, DateTimeKind.Utc).AddTicks(7158), "wDD96N0EYi3AkZgPQ7NQrCHzgca2gUfjvp4BIuEbzCQ=", "gr/glWIHMYD13C5jpSwuEccI88h68+SIxVwMhIGB/bo=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 20, 4, 54, 27, 810, DateTimeKind.Utc).AddTicks(8406), "8VzVC8ZkIKucKf3PLFSWELantOpEDs13PZuntFtTh+E=", "bRAeKdzBVyMwe5HEEL7ZHXcvFy4ZP34parKCeM5zzeI=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 20, 4, 54, 27, 810, DateTimeKind.Utc).AddTicks(8411), "+Dt2sQ5ZEIwA393R0hDJKrhlAYPiPutwrA2ISA5tveE=", "PaxPOQo1d3lG18iW3dwXsoNM5fOMujAfVJkVrEgQj3Y=" });
        }
    }
}

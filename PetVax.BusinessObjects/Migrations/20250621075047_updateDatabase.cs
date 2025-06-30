using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PetId",
                table: "MicrochipItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 21, 7, 50, 46, 535, DateTimeKind.Utc).AddTicks(3152), "QHVk/pGFGTqVRmU597gwK0qwYRpfUWjFjVmlsmHy78A=", "LrWwok/U96zQzcFfLuakFifOUWCsbBw4ph+4TtsAG6U=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 21, 7, 50, 46, 535, DateTimeKind.Utc).AddTicks(3157), "EbX+1rPVJprcAOIS81LF+u1Osva2M2Ykikh0PvAcc2k=", "JeCSdRxOVyDGZgACoU/wNLdwhTtd3iemIDCr/oP2Ffg=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PetId",
                table: "MicrochipItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase_isDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 21, 10, 7, 47, 491, DateTimeKind.Utc).AddTicks(752), "UkjQaOuM3bNpMP5obR63pVt9I3b2G8BLq6ooCddcFRE=", "c19b4CSUUxRbTUOIRoOxVgKJ2jaatv8sbIBqosHIo3U=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 21, 10, 7, 47, 491, DateTimeKind.Utc).AddTicks(760), "wpsjjCo2zAh3ddMksdTnNpJXDnzEUhOu5aZrWS+HKKQ=", "ur1TPv+egL/dcKThWtS6SPlYN5F/MsSM7Xoy0WAs/I4=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}

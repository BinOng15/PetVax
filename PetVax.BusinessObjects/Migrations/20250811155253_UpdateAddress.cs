using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 11, 15, 52, 52, 914, DateTimeKind.Utc).AddTicks(5957), "Gh8TFQdryMXNQiFypwCNA2tCV6/yf2UbQOM4J5hIre0=", "7P1+/y3U03YWiz7SryUU7g7aA3hZMmPr4+149wY4BzU=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 11, 15, 52, 52, 914, DateTimeKind.Utc).AddTicks(5962), "cdp8+GOeI9LGxR+mPFd0QpHQGtB+Ez6n8R97drO1mxU=", "tn1Q+5qZrsTFj/BdfxVkbGsJ57zdYgcHhtfZZXyoyUA=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 11, 15, 28, 4, 129, DateTimeKind.Utc).AddTicks(1868), "lG8mJYUsQYbkSJlNCb2lW3KUX4kByEq2Atl46+vBf6A=", "SjfvbytGb122RPQBbceUugeIj3wRHbMdx9NcY6DEz6s=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 11, 15, 28, 4, 129, DateTimeKind.Utc).AddTicks(1872), "HoPNCduxOV6chlJGtRkd4yudjqdNx5lpz6YEdTx8z8Y=", "SdaMAZwaT/78KO0MDYXBNe11xl0+zAZ0xhEcqOjkANE=" });
        }
    }
}

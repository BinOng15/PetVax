using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class Location_microchip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "MicrochipItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 28, 3, 20, 7, 250, DateTimeKind.Utc).AddTicks(7662), "lusxvYfKNMavUZlW09aXjobS9gukrR2sLYIU819MhbA=", "VyR+Wycf9GZk+uAYNz1zN7z+ePxaPP38n53GjoDz9gg=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 28, 3, 20, 7, 250, DateTimeKind.Utc).AddTicks(7667), "AQwFp7Y/2ZSJbilBkqDni69480oJ5YS+bodL5c1zWPw=", "LXqaMm0/2q8BjhoFrVccbfAgY9yZXEJ/GlEcdUvecuM=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "MicrochipItem");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 25, 16, 12, 2, 760, DateTimeKind.Utc).AddTicks(2758), "vp1UB0tWfvY8O+3oOLsaTWGUkAqHKCt3VwCR+aNe0rQ=", "539egAEbuX8Xqywpt4GQHLodNyX20DKsczCJlWoOlbQ=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 25, 16, 12, 2, 760, DateTimeKind.Utc).AddTicks(2764), "3S9N4PY+glIsYTtPB2rZmytgU5hzxKYPlJMxpkp8bKQ=", "hr/C+wxsaL7tZDdQhEdGZKuoEFD8Ol+WW0SLM+DBP9I=" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class week6 : Migration
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
                values: new object[] { new DateTime(2025, 6, 16, 6, 16, 18, 606, DateTimeKind.Utc).AddTicks(3545), "OnIX6H8p7nN7drJqocSchPH+JtXtOtK0/tqhR5c3dJs=", "emWZCN7wwu/4WgIyeukUthmG3UxeeiHRpFZFZKPG8Lc=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 16, 18, 606, DateTimeKind.Utc).AddTicks(3548), "98Vh5C6MQmqql3am4ckvyvZu7Di1Nb0GLqUiH05EL1E=", "MOmRz5rb2a4dVCN6P7mTLp3aVxTJMyRczrOallIlGl8=" });
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
                values: new object[] { new DateTime(2025, 6, 16, 5, 37, 57, 883, DateTimeKind.Utc).AddTicks(5137), "Rb6g//sujEa8Zl5uYOZEqVuNKN3YKZLd20kHbErM4Pg=", "IC8cazfkiRRH+BsAj6YKKCTla9LKfQ7t0PIsqu03tN8=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 16, 5, 37, 57, 883, DateTimeKind.Utc).AddTicks(5142), "xNuy3n4I9++9xBjPKvk+vA+BBVeAv4ZHfR6/v/WJVAs=", "AGb7Ag0tiNCNoHzqe/XumDbxxbiXcVXyEZ1TCM8F7EQ=" });
        }
    }
}

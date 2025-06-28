using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class newServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 27, 18, 2, 30, 140, DateTimeKind.Utc).AddTicks(2857), "4w9RDyZ/abB4U9AT5f0qqDfiRK94Thf9iIfxuDM27nk=", "wDWrEgKoXk6h5RRqu/QCgaZ+5fNGiyA78KeEIL2NPGM=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 27, 18, 2, 30, 140, DateTimeKind.Utc).AddTicks(2862), "xz4hAorKXgeDNRP3ckBV18bxCGoeXmVYNK6a7KM7Rq8=", "JpbNx7hCxCwDZ+mVyKn7p0XBzneW0pMRQdY9NFOLBik=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

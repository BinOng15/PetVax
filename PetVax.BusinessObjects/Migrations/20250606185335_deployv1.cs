using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class deployv1 : Migration
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
                values: new object[] { new DateTime(2025, 6, 6, 18, 53, 34, 834, DateTimeKind.Utc).AddTicks(9349), "9BPVobbIcXWO4VI9X8q8ZQpLyFFhKigXJKKYn2e9L1s=", "ChnfhkJx3fu+r2FNTdOsz5vaCiz0/cC7IUbAnUknV/Y=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 6, 18, 53, 34, 834, DateTimeKind.Utc).AddTicks(9354), "khJDtR17Ug/dIUHxfGzBwOsNj9CpBERN/zKLHbhIW6M=", "YJdQ94Uzwu3pj+jdfLxUN7y6jG+ONbc6kSGK35c4pAE=" });
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
                values: new object[] { new DateTime(2025, 6, 6, 16, 27, 13, 774, DateTimeKind.Utc).AddTicks(9096), "3/grvM501nv9q0SdviLbKzAyozK7TC7eiUPlijsf4HM=", "/i0zay2uP96I3g3GyMs8mBUI7zfPQ+VVH0MvDoLYrCM=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 6, 16, 27, 13, 774, DateTimeKind.Utc).AddTicks(9104), "S/BDGGmNUzmHqucOuUYZpVlRn4d1k7i92jlIuuJbNOM=", "+YOenQ/oiFrLhADqwTyhovdvF5xV3O/6E7D3IXP3Ke8=" });
        }
    }
}

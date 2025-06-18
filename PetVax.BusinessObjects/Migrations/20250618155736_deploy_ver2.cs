using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class deploy_ver2 : Migration
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
                values: new object[] { new DateTime(2025, 6, 18, 15, 57, 36, 73, DateTimeKind.Utc).AddTicks(88), "zFAJUjhTUQ1m3WutsVuKsgcFO4cBnoyha3BNtFdN3B0=", "fkHkj8MxLIcn7epqkB+HmSuownQwmxU7W+aYjhTnpEU=" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 15, 57, 36, 73, DateTimeKind.Utc).AddTicks(92), "0xIJ/2zlbisNDttDGXBFOSRk8msYcvB9U1rMUsZSuao=", "DbDiLmpcvRgF+YcLRj2ej8er+6UA57HRqqiDAyHFyK8=" });
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
                values: new object[] { new DateTime(2025, 6, 18, 15, 56, 7, 618, DateTimeKind.Utc).AddTicks(7725), "XFl7JT5F2jPhRgCIHkwuPwdSHy3p3BBbbaIfjeWspEY=", "OCiwRXWZd919K0gjY/3CW1SAKgHJtXquCDR7IpyEGrg=" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 15, 56, 7, 618, DateTimeKind.Utc).AddTicks(7730), "efRl1vz1utrOBtUp4egR/t1kPc+VnzC7EUglt3sHUD4=", "CUhIT4cMcs2Gl8TYQrkPon3xj1VRfEHKUxpEsLOL/40=" });
        }
    }
}

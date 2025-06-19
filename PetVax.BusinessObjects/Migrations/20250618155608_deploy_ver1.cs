using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class deploy_ver1 : Migration
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
                values: new object[] { new DateTime(2025, 6, 18, 15, 56, 7, 618, DateTimeKind.Utc).AddTicks(7725), "XFl7JT5F2jPhRgCIHkwuPwdSHy3p3BBbbaIfjeWspEY=", "OCiwRXWZd919K0gjY/3CW1SAKgHJtXquCDR7IpyEGrg=" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 15, 56, 7, 618, DateTimeKind.Utc).AddTicks(7730), "efRl1vz1utrOBtUp4egR/t1kPc+VnzC7EUglt3sHUD4=", "CUhIT4cMcs2Gl8TYQrkPon3xj1VRfEHKUxpEsLOL/40=" });
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
                values: new object[] { new DateTime(2025, 6, 18, 15, 54, 56, 706, DateTimeKind.Utc).AddTicks(88), "yEY3pEsk2a9WIlE3CCs2NRwQ4gpMO3cURCV/sjstXA4=", "OEof8k5jDxKDJrL0XZVcXcG/5apuqFqBSKnAoX/y0y0=" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 18, 15, 54, 56, 706, DateTimeKind.Utc).AddTicks(93), "4t+m+AjulwRBE0q0vi+ylUYLSOLuXK7dVi9BH5IN8PI=", "8bYVXt2JE0r6hbOtzh7lh7URzaZXtopQYSNKRIMgSsw=" });
        }
    }
}

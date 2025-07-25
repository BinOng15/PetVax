using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipBetweenCustomerVsMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customer_MembershipId",
                table: "Customer");

            migrationBuilder.AddColumn<int>(
                name: "PointsRequired",
                table: "Voucher",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MinPoints",
                table: "Membership",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 14, 22, 17, 118, DateTimeKind.Utc).AddTicks(2564), "vjrOyijB70Nwz7f6zuNAebOW258VXpEWpo4dS6HjwE8=", "zo0Zwi8FZfhN7R7dNFTPFpI9pwJeJhmiB9d2m9620a0=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 24, 14, 22, 17, 118, DateTimeKind.Utc).AddTicks(2568), "YefIM0pUQ0FoCEUKHTdKMFFK71FhGIQJFF/K5l4FYQk=", "ohkzlNgMZ3H0n0uiOyCFzZAHN44O4v1nNPrve5VXNGg=" });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_MembershipId",
                table: "Customer",
                column: "MembershipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customer_MembershipId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "PointsRequired",
                table: "Voucher");

            migrationBuilder.AlterColumn<string>(
                name: "MinPoints",
                table: "Membership",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 22, 7, 43, 47, 258, DateTimeKind.Utc).AddTicks(3965), "AqD7kRxgZafHFUIDEB9MCAJHS4/p/+lAr9W59d9uzWw=", "gBLJMX/zHFnePiEOLwMqeJlYXeDtrcHptCbxtsu61kY=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 22, 7, 43, 47, 258, DateTimeKind.Utc).AddTicks(3978), "cTSyKpm9HW8P/aDzd+vUMBLMoKJw0hLMJvoQNXZrDXU=", "Rsr2JOrtV2fjsID5GWN9YJ6X4N3Hm+IEiAXCaR0AHjU=" });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_MembershipId",
                table: "Customer",
                column: "MembershipId",
                unique: true,
                filter: "[MembershipId] IS NOT NULL");
        }
    }
}

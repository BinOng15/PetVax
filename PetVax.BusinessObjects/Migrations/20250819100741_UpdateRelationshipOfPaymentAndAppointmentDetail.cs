using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipOfPaymentAndAppointmentDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_AppointmentDetailId",
                table: "Payment");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 19, 10, 7, 40, 480, DateTimeKind.Utc).AddTicks(852), "HoJzl9v+5jToIKzkP2TBbHOPXA7QByhUNgS0XEfPSqY=", "dZKalaCdwaetF/DzLbiUL/Bs2RLP0lH99oT30LPKpHE=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 19, 10, 7, 40, 480, DateTimeKind.Utc).AddTicks(859), "vov7kgCApeJMM3C2b8aD6PEUDiXh13h9jw7cQhLl4I0=", "9vjFxsJS2Lj4NiGAf799oWRycOn2wD7oKokvm6DTjgo=" });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_AppointmentDetailId",
                table: "Payment",
                column: "AppointmentDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_AppointmentDetailId",
                table: "Payment");

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

            migrationBuilder.CreateIndex(
                name: "IX_Payment_AppointmentDetailId",
                table: "Payment",
                column: "AppointmentDetailId",
                unique: true);
        }
    }
}

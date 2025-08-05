using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class update_databaseServiceHistory3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "ServiceHistory",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "ServiceHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "ServiceHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "ServiceHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 31, 2, 28, 53, 490, DateTimeKind.Utc).AddTicks(9256), "wp0dFE5zyxSXF5/Qso0yb1sKDQQY73dZEagqYHKsUyE=", "s+9iOOgFe+urV8S9Yc/NG++778Ngr9VUiN/aZrBIDzs=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 31, 2, 28, 53, 490, DateTimeKind.Utc).AddTicks(9262), "9bIJEQIRW6eDVvZLX14jTH2pB6u9UyHj/o8JcTcw7tc=", "++zYm25Dg7p1NltxNSSgzSewTjwd+sxN5fSwXVZg/aE=" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistory_AppointmentId",
                table: "ServiceHistory",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistory_PetId",
                table: "ServiceHistory",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceHistory_Appointment_AppointmentId",
                table: "ServiceHistory",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceHistory_Pet_PetId",
                table: "ServiceHistory",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceHistory_Appointment_AppointmentId",
                table: "ServiceHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceHistory_Pet_PetId",
                table: "ServiceHistory");

            migrationBuilder.DropIndex(
                name: "IX_ServiceHistory_AppointmentId",
                table: "ServiceHistory");

            migrationBuilder.DropIndex(
                name: "IX_ServiceHistory_PetId",
                table: "ServiceHistory");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ServiceHistory");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "ServiceHistory");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "ServiceHistory");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "ServiceHistory");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 27, 16, 5, 23, 374, DateTimeKind.Utc).AddTicks(8485), "djYJdXtkHUI89pvit8YGwQfamXqeUrzQnbxz7Zfyq60=", "KxU7SS0fp9/JTLt1inEcRZmKdfs9nHOS5QRp/MNMC+s=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 27, 16, 5, 23, 374, DateTimeKind.Utc).AddTicks(8497), "lE+K91fxLReO2DjRJntsPOFnTTu5w0AeHsF6rGfFJ2s=", "QBq6zHCeir6Xy5YzjDPEs7zcYKFLmKs4VsP5Vp2zLhI=" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVer12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiseaseId",
                schema: "dbo",
                table: "AppointmentDetail",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 17, 9, 9, 2, 265, DateTimeKind.Utc).AddTicks(9611), "aYoOFGIrwzJur8jU2/g956N+eByL0PtPrNs8ZqFWNmo=", "wTYlwBtruLSWXs/GY9KLiOso+qjMv2W/OfUZwmwv0xM=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 17, 9, 9, 2, 265, DateTimeKind.Utc).AddTicks(9619), "ja8Pm6QEzLZCN7dKOUqDKJJ/5IRZQqs5zqSzHy2J9Gs=", "eJUmbYh4vvy1ZJmnpyG1I1B8J6ySojHONPejN9MNfYQ=" });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_DiseaseId",
                schema: "dbo",
                table: "AppointmentDetail",
                column: "DiseaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDetail_Disease_DiseaseId",
                schema: "dbo",
                table: "AppointmentDetail",
                column: "DiseaseId",
                principalSchema: "dbo",
                principalTable: "Disease",
                principalColumn: "DiseaseId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDetail_Disease_DiseaseId",
                schema: "dbo",
                table: "AppointmentDetail");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_DiseaseId",
                schema: "dbo",
                table: "AppointmentDetail");

            migrationBuilder.DropColumn(
                name: "DiseaseId",
                schema: "dbo",
                table: "AppointmentDetail");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 17, 8, 28, 46, 774, DateTimeKind.Utc).AddTicks(7690), "Uu/X1Cxh/G3N0oGuHtVS3uRalzSRPLavXIKUubWlF/E=", "zFkRw68p5FVa1waTJoJr2PsaOhj0bzUUBCR17924opw=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 17, 8, 28, 46, 774, DateTimeKind.Utc).AddTicks(7696), "Zwps6hMGVQwP+X2e1MHeG5+H34bNb80OZVKtg9KGkBU=", "xHtkFu8xzi1mYYgNu6chfwsM3okkmULiY4I7APfQWeE=" });
        }
    }
}

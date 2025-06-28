using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class FixVaccineBatchRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_VaccineBatchId",
                table: "AppointmentDetail");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 28, 3, 47, 11, 949, DateTimeKind.Utc).AddTicks(561), "DCDbfVxKuc0W3XwDlEAVAydMNcXVQbxUSE97ZzfQNcU=", "Bd8S/dS3cC4MU8glbNPYG85wvj8P1hJ6AxZ59r8XHmI=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 28, 3, 47, 11, 949, DateTimeKind.Utc).AddTicks(564), "eMEKT5F2eKx6eYWT+cg+cLYwKdBFyPFRi7E426DGP3w=", "kxaVP3sGcUe4CJXqKyltH4gEjFVAMBENg6OiaBeVcDk=" });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_VaccineBatchId",
                table: "AppointmentDetail",
                column: "VaccineBatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_VaccineBatchId",
                table: "AppointmentDetail");

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

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_VaccineBatchId",
                table: "AppointmentDetail",
                column: "VaccineBatchId",
                unique: true,
                filter: "[VaccineBatchId] IS NOT NULL");
        }
    }
}

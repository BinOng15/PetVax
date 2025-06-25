using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueIndex_VaccineProfile_PetId_DiseaseId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VaccineProfile_PetId_DiseaseId",
                table: "VaccineProfile");

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

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfile_PetId",
                table: "VaccineProfile",
                column: "PetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VaccineProfile_PetId",
                table: "VaccineProfile");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 25, 16, 0, 45, 495, DateTimeKind.Utc).AddTicks(7808), "P/MBXlC7CBtwL2Qrd+5OUfYYDIVE8awGWgFeeZYWFxc=", "/bbk5li4Tpzpz00xMaD3yoDZNNxxRFiy37QdzEIIwFE=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 25, 16, 0, 45, 495, DateTimeKind.Utc).AddTicks(7812), "DMuPx1kZ7ViCKQ8EhKdlwmBfnboXAj2HO+wmgKkEVic=", "dDSrn3AD2hoNFoBNE0n21vt+Rd80H6LKoLEPn9jfAyo=" });

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfile_PetId_DiseaseId",
                table: "VaccineProfile",
                columns: new[] { "PetId", "DiseaseId" },
                unique: true,
                filter: "[DiseaseId] IS NOT NULL");
        }
    }
}

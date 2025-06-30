using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class PetHasManyVaccineProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VaccineProfile_PetId",
                table: "VaccineProfile");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 24, 16, 42, 0, 937, DateTimeKind.Utc).AddTicks(8717), "JH8OviZ/aVyjUc5Xp80rs/RbXiKuoSCe7lrTZcT2rd0=", "tKpbaR3m6mLRh2lEES8ZK3AtA7o2FvZEN1F0ok+KHBU=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 24, 16, 42, 0, 937, DateTimeKind.Utc).AddTicks(8725), "h5gslHEPo8Ef25gmsnpY3ApHS85KdoZ31Tju1W6Oa/c=", "cacXCr7RsaTXWhpVi67QE4HKmQuX/Qv51RjXpN8AXBA=" });

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfile_PetId_DiseaseId",
                table: "VaccineProfile",
                columns: new[] { "PetId", "DiseaseId" },
                unique: true,
                filter: "[DiseaseId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VaccineProfile_PetId_DiseaseId",
                table: "VaccineProfile");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 24, 15, 33, 28, 770, DateTimeKind.Utc).AddTicks(3090), "9Y/ypNXmReEWWMFHvlUicIAaa6raOl11bPdMHW9lyWE=", "/4w7YoZxudFxFh7Sy7Zkl7D/+lrxjA7mz8myIEYLv8w=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 24, 15, 33, 28, 770, DateTimeKind.Utc).AddTicks(3095), "PA5r4uqPhjow8paofyWSW7qAYsLGgGFaNsAk9qr/2UY=", "gkBp7NdHgPXiqXdrhpQffIc3KnkFoA5WB9vOPPB+i7s=" });

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfile_PetId",
                table: "VaccineProfile",
                column: "PetId",
                unique: true);
        }
    }
}

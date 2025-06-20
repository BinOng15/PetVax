using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class updateVaccineProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccineProfile_Disease_DiseaseId",
                table: "VaccineProfile");

            migrationBuilder.DropIndex(
                name: "IX_VaccineProfile_DiseaseId",
                table: "VaccineProfile");

            migrationBuilder.DropColumn(
                name: "DiseaseId",
                table: "VaccineProfile");

            migrationBuilder.CreateTable(
                name: "VaccineProfileDisease",
                columns: table => new
                {
                    VaccineProfileDiseasesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VaccineProfileId = table.Column<int>(type: "int", nullable: false),
                    DiseaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineProfileDisease", x => x.VaccineProfileDiseasesId);
                    table.ForeignKey(
                        name: "FK_VaccineProfileDisease_Disease_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Disease",
                        principalColumn: "DiseaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccineProfileDisease_VaccineProfile_VaccineProfileId",
                        column: x => x.VaccineProfileId,
                        principalTable: "VaccineProfile",
                        principalColumn: "VaccineProfileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 20, 4, 18, 27, 909, DateTimeKind.Utc).AddTicks(4115), "Dgfh2wQOJnNC/frYzubEQ4tolWjTSknB5QbU/g/DwVI=", "S/B15Dte8OS1ebk+LejlI1mxntSTwmdBiCFOwmXlOtI=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 20, 4, 18, 27, 909, DateTimeKind.Utc).AddTicks(4123), "CscrmXOnndGByCni+eD80fnPGq0fqmvjTghwEx7udQM=", "sgcrtpJdOEYZwcV7oJIJ586gAECI7oGFvKlsFeqH9/8=" });

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfileDisease_DiseaseId",
                table: "VaccineProfileDisease",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfileDisease_VaccineProfileId",
                table: "VaccineProfileDisease",
                column: "VaccineProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VaccineProfileDisease");

            migrationBuilder.AddColumn<int>(
                name: "DiseaseId",
                table: "VaccineProfile",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 19, 13, 52, 21, 227, DateTimeKind.Utc).AddTicks(6232), "YQ7id8GLiyD5+PRRirkre22mHbi5cCLHVOiX26+ap0U=", "9J2pG6R6nbWtl4mKTrqf6/WT0ACbtYJVG6nLTffeLPA=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 19, 13, 52, 21, 227, DateTimeKind.Utc).AddTicks(6236), "90EYaEPp0F1H/3NCx94teSv73p41+NMCv/9IUlM6Hms=", "dQo+8kwoakfTAMCzwjk0d63CQMY9dk79EcxoH/sB74g=" });

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfile_DiseaseId",
                table: "VaccineProfile",
                column: "DiseaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineProfile_Disease_DiseaseId",
                table: "VaccineProfile",
                column: "DiseaseId",
                principalTable: "Disease",
                principalColumn: "DiseaseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

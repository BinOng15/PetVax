using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class FixPassportToCertificate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDetail_PetPassport_PassportId",
                table: "AppointmentDetail");

            migrationBuilder.DropTable(
                name: "PetPassport");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_PassportId",
                table: "AppointmentDetail");

            migrationBuilder.RenameColumn(
                name: "PassportId",
                table: "AppointmentDetail",
                newName: "VaccinationCertificateId");

            migrationBuilder.AddColumn<int>(
                name: "MicrochipItemId",
                table: "HealthCondition",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VetId",
                table: "HealthCondition",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VaccinationCertificate",
                columns: table => new
                {
                    CertificateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetId = table.Column<int>(type: "int", nullable: false),
                    MicrochipItemId = table.Column<int>(type: "int", nullable: false),
                    VetId = table.Column<int>(type: "int", nullable: false),
                    DiseaseId = table.Column<int>(type: "int", nullable: false),
                    CertificateCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoseNumber = table.Column<int>(type: "int", nullable: false),
                    VaccinationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClinicName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClinicAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationCertificate", x => x.CertificateId);
                    table.ForeignKey(
                        name: "FK_VaccinationCertificate_Disease_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Disease",
                        principalColumn: "DiseaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccinationCertificate_MicrochipItem_MicrochipItemId",
                        column: x => x.MicrochipItemId,
                        principalTable: "MicrochipItem",
                        principalColumn: "MicrochipItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccinationCertificate_Pet_PetId",
                        column: x => x.PetId,
                        principalTable: "Pet",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccinationCertificate_Vet_VetId",
                        column: x => x.VetId,
                        principalTable: "Vet",
                        principalColumn: "VetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HealthConditionVaccinationCertificate",
                columns: table => new
                {
                    HealthConditionVaccinationCertificateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthConditionId = table.Column<int>(type: "int", nullable: false),
                    VaccinationCertificateId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthConditionVaccinationCertificate", x => x.HealthConditionVaccinationCertificateId);
                    table.ForeignKey(
                        name: "FK_HealthConditionVaccinationCertificate_HealthCondition_HealthConditionId",
                        column: x => x.HealthConditionId,
                        principalTable: "HealthCondition",
                        principalColumn: "HealthConditionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthConditionVaccinationCertificate_VaccinationCertificate_VaccinationCertificateId",
                        column: x => x.VaccinationCertificateId,
                        principalTable: "VaccinationCertificate",
                        principalColumn: "CertificateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 19, 50, 19, DateTimeKind.Utc).AddTicks(6213), "/I2qqoaqPcjJDuycDNo4IlLrKolZTGUn9LWz41DnXdA=", "hU5FdUdzNr+OUWMT3iwdE3pt7h/pXfv6QVNcNWRyeto=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 19, 50, 19, DateTimeKind.Utc).AddTicks(6220), "3jtuEowq4CaYH5BDjodBdzqP7J8xYAuSq9H/kU3jkD8=", "6YE9NdBBEKc6QDmoJn/dfRV5BhBuWPd6pjan+Zqcnw4=" });

            migrationBuilder.CreateIndex(
                name: "IX_HealthCondition_MicrochipItemId",
                table: "HealthCondition",
                column: "MicrochipItemId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCondition_VetId",
                table: "HealthCondition",
                column: "VetId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_VaccinationCertificateId",
                table: "AppointmentDetail",
                column: "VaccinationCertificateId",
                unique: true,
                filter: "[VaccinationCertificateId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HealthConditionVaccinationCertificate_HealthConditionId",
                table: "HealthConditionVaccinationCertificate",
                column: "HealthConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthConditionVaccinationCertificate_VaccinationCertificateId",
                table: "HealthConditionVaccinationCertificate",
                column: "VaccinationCertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationCertificate_DiseaseId",
                table: "VaccinationCertificate",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationCertificate_MicrochipItemId",
                table: "VaccinationCertificate",
                column: "MicrochipItemId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationCertificate_PetId",
                table: "VaccinationCertificate",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationCertificate_VetId",
                table: "VaccinationCertificate",
                column: "VetId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDetail_VaccinationCertificate_VaccinationCertificateId",
                table: "AppointmentDetail",
                column: "VaccinationCertificateId",
                principalTable: "VaccinationCertificate",
                principalColumn: "CertificateId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCondition_MicrochipItem_MicrochipItemId",
                table: "HealthCondition",
                column: "MicrochipItemId",
                principalTable: "MicrochipItem",
                principalColumn: "MicrochipItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCondition_Vet_VetId",
                table: "HealthCondition",
                column: "VetId",
                principalTable: "Vet",
                principalColumn: "VetId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDetail_VaccinationCertificate_VaccinationCertificateId",
                table: "AppointmentDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCondition_MicrochipItem_MicrochipItemId",
                table: "HealthCondition");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCondition_Vet_VetId",
                table: "HealthCondition");

            migrationBuilder.DropTable(
                name: "HealthConditionVaccinationCertificate");

            migrationBuilder.DropTable(
                name: "VaccinationCertificate");

            migrationBuilder.DropIndex(
                name: "IX_HealthCondition_MicrochipItemId",
                table: "HealthCondition");

            migrationBuilder.DropIndex(
                name: "IX_HealthCondition_VetId",
                table: "HealthCondition");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_VaccinationCertificateId",
                table: "AppointmentDetail");

            migrationBuilder.DropColumn(
                name: "MicrochipItemId",
                table: "HealthCondition");

            migrationBuilder.DropColumn(
                name: "VetId",
                table: "HealthCondition");

            migrationBuilder.RenameColumn(
                name: "VaccinationCertificateId",
                table: "AppointmentDetail",
                newName: "PassportId");

            migrationBuilder.CreateTable(
                name: "PetPassport",
                columns: table => new
                {
                    PassportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthConditionId = table.Column<int>(type: "int", nullable: false),
                    MicrochipItemId = table.Column<int>(type: "int", nullable: false),
                    PetId = table.Column<int>(type: "int", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HealthCheckDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassportImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaccinationDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true),
                    isRabiesVaccinated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetPassport", x => x.PassportId);
                    table.ForeignKey(
                        name: "FK_PetPassport_HealthCondition_HealthConditionId",
                        column: x => x.HealthConditionId,
                        principalTable: "HealthCondition",
                        principalColumn: "HealthConditionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PetPassport_MicrochipItem_MicrochipItemId",
                        column: x => x.MicrochipItemId,
                        principalTable: "MicrochipItem",
                        principalColumn: "MicrochipItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PetPassport_Pet_PetId",
                        column: x => x.PetId,
                        principalTable: "Pet",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 28, 3, 20, 7, 250, DateTimeKind.Utc).AddTicks(7662), "lusxvYfKNMavUZlW09aXjobS9gukrR2sLYIU819MhbA=", "VyR+Wycf9GZk+uAYNz1zN7z+ePxaPP38n53GjoDz9gg=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 28, 3, 20, 7, 250, DateTimeKind.Utc).AddTicks(7667), "AQwFp7Y/2ZSJbilBkqDni69480oJ5YS+bodL5c1zWPw=", "LXqaMm0/2q8BjhoFrVccbfAgY9yZXEJ/GlEcdUvecuM=" });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_PassportId",
                table: "AppointmentDetail",
                column: "PassportId",
                unique: true,
                filter: "[PassportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PetPassport_HealthConditionId",
                table: "PetPassport",
                column: "HealthConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_PetPassport_MicrochipItemId",
                table: "PetPassport",
                column: "MicrochipItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PetPassport_PetId",
                table: "PetPassport",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDetail_PetPassport_PassportId",
                table: "AppointmentDetail",
                column: "PassportId",
                principalTable: "PetPassport",
                principalColumn: "PassportId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

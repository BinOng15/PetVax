using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Account",
                schema: "dbo",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefereshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Disease",
                schema: "dbo",
                columns: table => new
                {
                    DiseaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Species = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symptoms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Treatment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disease", x => x.DiseaseId);
                });

            migrationBuilder.CreateTable(
                name: "Membership",
                schema: "dbo",
                columns: table => new
                {
                    MembershipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MembershipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinPoints = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Benefits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membership", x => x.MembershipId);
                });

            migrationBuilder.CreateTable(
                name: "Microchip",
                schema: "dbo",
                columns: table => new
                {
                    MicrochipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MicrochipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Microchip", x => x.MicrochipId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceHistory",
                schema: "dbo",
                columns: table => new
                {
                    ServiceHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceHistory", x => x.ServiceHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "Vaccine",
                schema: "dbo",
                columns: table => new
                {
                    VaccineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VaccineCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccine", x => x.VaccineId);
                });

            migrationBuilder.CreateTable(
                name: "Vet",
                schema: "dbo",
                columns: table => new
                {
                    VetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    VetCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vet", x => x.VetId);
                    table.ForeignKey(
                        name: "FK_Vet_Account_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "dbo",
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VaccinationSchedule",
                schema: "dbo",
                columns: table => new
                {
                    VaccinationScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiseaseId = table.Column<int>(type: "int", nullable: false),
                    Species = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoseNumber = table.Column<int>(type: "int", nullable: false),
                    AgeInterval = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationSchedule", x => x.VaccinationScheduleId);
                    table.ForeignKey(
                        name: "FK_VaccinationSchedule_Disease_DiseaseId",
                        column: x => x.DiseaseId,
                        principalSchema: "dbo",
                        principalTable: "Disease",
                        principalColumn: "DiseaseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "dbo",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    MembershipId = table.Column<int>(type: "int", nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentPoints = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customer_Account_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "dbo",
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customer_Membership_MembershipId",
                        column: x => x.MembershipId,
                        principalSchema: "dbo",
                        principalTable: "Membership",
                        principalColumn: "MembershipId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VaccineBatch",
                schema: "dbo",
                columns: table => new
                {
                    VaccineBatchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VaccineId = table.Column<int>(type: "int", nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufactureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineBatch", x => x.VaccineBatchId);
                    table.ForeignKey(
                        name: "FK_VaccineBatch_Vaccine_VaccineId",
                        column: x => x.VaccineId,
                        principalSchema: "dbo",
                        principalTable: "Vaccine",
                        principalColumn: "VaccineId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VaccineDisease",
                schema: "dbo",
                columns: table => new
                {
                    VaccineDiseaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VaccineId = table.Column<int>(type: "int", nullable: false),
                    DiseaseId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineDisease", x => x.VaccineDiseaseId);
                    table.ForeignKey(
                        name: "FK_VaccineDisease_Disease_DiseaseId",
                        column: x => x.DiseaseId,
                        principalSchema: "dbo",
                        principalTable: "Disease",
                        principalColumn: "DiseaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccineDisease_Vaccine_VaccineId",
                        column: x => x.VaccineId,
                        principalSchema: "dbo",
                        principalTable: "Vaccine",
                        principalColumn: "VaccineId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VetSchedule",
                schema: "dbo",
                columns: table => new
                {
                    VetScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VetId = table.Column<int>(type: "int", nullable: false),
                    ScheduleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SlotNumber = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VetSchedule", x => x.VetScheduleId);
                    table.ForeignKey(
                        name: "FK_VetSchedule_Vet_VetId",
                        column: x => x.VetId,
                        principalSchema: "dbo",
                        principalTable: "Vet",
                        principalColumn: "VetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pet",
                schema: "dbo",
                columns: table => new
                {
                    PetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    PetCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Species = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Breed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isSterilized = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.PetId);
                    table.ForeignKey(
                        name: "FK_Pet_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PointTransaction",
                schema: "dbo",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Change = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointTransaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_PointTransaction_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VaccineExport",
                schema: "dbo",
                columns: table => new
                {
                    VaccineExportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExportCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VaccineBatchId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineExport", x => x.VaccineExportId);
                    table.ForeignKey(
                        name: "FK_VaccineExport_VaccineBatch_VaccineBatchId",
                        column: x => x.VaccineBatchId,
                        principalSchema: "dbo",
                        principalTable: "VaccineBatch",
                        principalColumn: "VaccineBatchId");
                });

            migrationBuilder.CreateTable(
                name: "VaccineReceipt",
                schema: "dbo",
                columns: table => new
                {
                    VaccineReceiptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiptCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Suppiler = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VaccineBatchId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineReceipt", x => x.VaccineReceiptId);
                    table.ForeignKey(
                        name: "FK_VaccineReceipt_VaccineBatch_VaccineBatchId",
                        column: x => x.VaccineBatchId,
                        principalSchema: "dbo",
                        principalTable: "VaccineBatch",
                        principalColumn: "VaccineBatchId");
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                schema: "dbo",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    PetId = table.Column<int>(type: "int", nullable: false),
                    AppointmentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointment_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointment_Pet_PetId",
                        column: x => x.PetId,
                        principalSchema: "dbo",
                        principalTable: "Pet",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HealthCondition",
                schema: "dbo",
                columns: table => new
                {
                    HealthConditionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetId = table.Column<int>(type: "int", nullable: false),
                    ConditionCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeartRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BreathingRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Temperature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EHNM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkinAFur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Digestion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Respiratory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Excrete = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Behavior = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Psycho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Different = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conclusion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCondition", x => x.HealthConditionId);
                    table.ForeignKey(
                        name: "FK_HealthCondition_Pet_PetId",
                        column: x => x.PetId,
                        principalSchema: "dbo",
                        principalTable: "Pet",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MicrochipItem",
                schema: "dbo",
                columns: table => new
                {
                    MicrochipItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MicrochipId = table.Column<int>(type: "int", nullable: false),
                    PetId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstallationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MicrochipItem", x => x.MicrochipItemId);
                    table.ForeignKey(
                        name: "FK_MicrochipItem_Microchip_MicrochipId",
                        column: x => x.MicrochipId,
                        principalSchema: "dbo",
                        principalTable: "Microchip",
                        principalColumn: "MicrochipId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MicrochipItem_Pet_PetId",
                        column: x => x.PetId,
                        principalSchema: "dbo",
                        principalTable: "Pet",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VaccineProfile",
                schema: "dbo",
                columns: table => new
                {
                    VaccineProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetId = table.Column<int>(type: "int", nullable: false),
                    DiseaseId = table.Column<int>(type: "int", nullable: false),
                    PreferedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VaccinationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Dose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reaction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NextVaccinationInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VaccinationScheduleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineProfile", x => x.VaccineProfileId);
                    table.ForeignKey(
                        name: "FK_VaccineProfile_Disease_DiseaseId",
                        column: x => x.DiseaseId,
                        principalSchema: "dbo",
                        principalTable: "Disease",
                        principalColumn: "DiseaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccineProfile_Pet_PetId",
                        column: x => x.PetId,
                        principalSchema: "dbo",
                        principalTable: "Pet",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccineProfile_VaccinationSchedule_VaccinationScheduleId",
                        column: x => x.VaccinationScheduleId,
                        principalSchema: "dbo",
                        principalTable: "VaccinationSchedule",
                        principalColumn: "VaccinationScheduleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VaccineReceiptDetail",
                schema: "dbo",
                columns: table => new
                {
                    VaccineReceiptDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VaccineReceiptId = table.Column<int>(type: "int", nullable: false),
                    VaccineBatchId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineReceiptDetail", x => x.VaccineReceiptDetailId);
                    table.ForeignKey(
                        name: "FK_VaccineReceiptDetail_VaccineBatch_VaccineBatchId",
                        column: x => x.VaccineBatchId,
                        principalSchema: "dbo",
                        principalTable: "VaccineBatch",
                        principalColumn: "VaccineBatchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccineReceiptDetail_VaccineReceipt_VaccineReceiptId",
                        column: x => x.VaccineReceiptId,
                        principalSchema: "dbo",
                        principalTable: "VaccineReceipt",
                        principalColumn: "VaccineReceiptId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PetPassport",
                schema: "dbo",
                columns: table => new
                {
                    PassportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetId = table.Column<int>(type: "int", nullable: false),
                    MicrochipItemId = table.Column<int>(type: "int", nullable: false),
                    HealthConditionId = table.Column<int>(type: "int", nullable: false),
                    PassportCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassportImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaccinationDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HealthCheckDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isRabiesVaccinated = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetPassport", x => x.PassportId);
                    table.ForeignKey(
                        name: "FK_PetPassport_HealthCondition_HealthConditionId",
                        column: x => x.HealthConditionId,
                        principalSchema: "dbo",
                        principalTable: "HealthCondition",
                        principalColumn: "HealthConditionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PetPassport_MicrochipItem_MicrochipItemId",
                        column: x => x.MicrochipItemId,
                        principalSchema: "dbo",
                        principalTable: "MicrochipItem",
                        principalColumn: "MicrochipItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PetPassport_Pet_PetId",
                        column: x => x.PetId,
                        principalSchema: "dbo",
                        principalTable: "Pet",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentDetail",
                schema: "dbo",
                columns: table => new
                {
                    AppointmentDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    VetId = table.Column<int>(type: "int", nullable: false),
                    VaccineProfileId = table.Column<int>(type: "int", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MicrochipItemId = table.Column<int>(type: "int", nullable: false),
                    PassportId = table.Column<int>(type: "int", nullable: false),
                    HealthConditionId = table.Column<int>(type: "int", nullable: false),
                    VaccineBatchId = table.Column<int>(type: "int", nullable: false),
                    AppointmentDetailCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reaction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NextVaccinationInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentDetail", x => x.AppointmentDetailId);
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalSchema: "dbo",
                        principalTable: "Appointment",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_HealthCondition_HealthConditionId",
                        column: x => x.HealthConditionId,
                        principalSchema: "dbo",
                        principalTable: "HealthCondition",
                        principalColumn: "HealthConditionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_MicrochipItem_MicrochipItemId",
                        column: x => x.MicrochipItemId,
                        principalSchema: "dbo",
                        principalTable: "MicrochipItem",
                        principalColumn: "MicrochipItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_PetPassport_PassportId",
                        column: x => x.PassportId,
                        principalSchema: "dbo",
                        principalTable: "PetPassport",
                        principalColumn: "PassportId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_ServiceHistory_ServiceHistoryId",
                        column: x => x.ServiceHistoryId,
                        principalSchema: "dbo",
                        principalTable: "ServiceHistory",
                        principalColumn: "ServiceHistoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_VaccineBatch_VaccineBatchId",
                        column: x => x.VaccineBatchId,
                        principalSchema: "dbo",
                        principalTable: "VaccineBatch",
                        principalColumn: "VaccineBatchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_VaccineProfile_VaccineProfileId",
                        column: x => x.VaccineProfileId,
                        principalSchema: "dbo",
                        principalTable: "VaccineProfile",
                        principalColumn: "VaccineProfileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_Vet_VetId",
                        column: x => x.VetId,
                        principalSchema: "dbo",
                        principalTable: "Vet",
                        principalColumn: "VetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                schema: "dbo",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentDetailId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    VaccineId = table.Column<int>(type: "int", nullable: false),
                    PaymentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payment_AppointmentDetail_AppointmentDetailId",
                        column: x => x.AppointmentDetailId,
                        principalSchema: "dbo",
                        principalTable: "AppointmentDetail",
                        principalColumn: "AppointmentDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payment_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payment_Vaccine_VaccineId",
                        column: x => x.VaccineId,
                        principalSchema: "dbo",
                        principalTable: "Vaccine",
                        principalColumn: "VaccineId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VaccineExportDetail",
                schema: "dbo",
                columns: table => new
                {
                    VaccineExportDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VaccineBatchId = table.Column<int>(type: "int", nullable: false),
                    VaccineExportId = table.Column<int>(type: "int", nullable: false),
                    AppointmentDetailId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineExportDetail", x => x.VaccineExportDetailId);
                    table.ForeignKey(
                        name: "FK_VaccineExportDetail_AppointmentDetail_AppointmentDetailId",
                        column: x => x.AppointmentDetailId,
                        principalSchema: "dbo",
                        principalTable: "AppointmentDetail",
                        principalColumn: "AppointmentDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccineExportDetail_VaccineBatch_VaccineBatchId",
                        column: x => x.VaccineBatchId,
                        principalSchema: "dbo",
                        principalTable: "VaccineBatch",
                        principalColumn: "VaccineBatchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccineExportDetail_VaccineExport_VaccineExportId",
                        column: x => x.VaccineExportId,
                        principalSchema: "dbo",
                        principalTable: "VaccineExport",
                        principalColumn: "VaccineExportId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Account",
                columns: new[] { "AccountId", "AccessToken", "CreatedAt", "CreatedBy", "Email", "ModifiedAt", "ModifiedBy", "Password", "RefereshToken", "Role" },
                values: new object[,]
                {
                    { 1, "", new DateTime(2025, 6, 3, 15, 25, 0, 584, DateTimeKind.Utc).AddTicks(517), "system", "admin@petvax.com", null, "", "tns+8qNsHzTeEu/NbYni0t43PI5KMl0gXbEEN8qjMoQ=", "", 1 },
                    { 2, "", new DateTime(2025, 6, 3, 15, 25, 0, 584, DateTimeKind.Utc).AddTicks(522), "system", "staff@petvax.com", null, "", "gYMd4KGBD2sEeUuepPrVJ6INHXlkOWjBzvJvU7rRON0=", "", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_CustomerId",
                schema: "dbo",
                table: "Appointment",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_PetId",
                schema: "dbo",
                table: "Appointment",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_AppointmentId",
                schema: "dbo",
                table: "AppointmentDetail",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_HealthConditionId",
                schema: "dbo",
                table: "AppointmentDetail",
                column: "HealthConditionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_MicrochipItemId",
                schema: "dbo",
                table: "AppointmentDetail",
                column: "MicrochipItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_PassportId",
                schema: "dbo",
                table: "AppointmentDetail",
                column: "PassportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_ServiceHistoryId",
                schema: "dbo",
                table: "AppointmentDetail",
                column: "ServiceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_VaccineBatchId",
                schema: "dbo",
                table: "AppointmentDetail",
                column: "VaccineBatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_VaccineProfileId",
                schema: "dbo",
                table: "AppointmentDetail",
                column: "VaccineProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_VetId",
                schema: "dbo",
                table: "AppointmentDetail",
                column: "VetId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_AccountId",
                schema: "dbo",
                table: "Customer",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_MembershipId",
                schema: "dbo",
                table: "Customer",
                column: "MembershipId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthCondition_PetId",
                schema: "dbo",
                table: "HealthCondition",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_MicrochipItem_MicrochipId",
                schema: "dbo",
                table: "MicrochipItem",
                column: "MicrochipId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MicrochipItem_PetId",
                schema: "dbo",
                table: "MicrochipItem",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_AppointmentDetailId",
                schema: "dbo",
                table: "Payment",
                column: "AppointmentDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_CustomerId",
                schema: "dbo",
                table: "Payment",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_VaccineId",
                schema: "dbo",
                table: "Payment",
                column: "VaccineId");

            migrationBuilder.CreateIndex(
                name: "IX_Pet_CustomerId",
                schema: "dbo",
                table: "Pet",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PetPassport_HealthConditionId",
                schema: "dbo",
                table: "PetPassport",
                column: "HealthConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_PetPassport_MicrochipItemId",
                schema: "dbo",
                table: "PetPassport",
                column: "MicrochipItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PetPassport_PetId",
                schema: "dbo",
                table: "PetPassport",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransaction_CustomerId",
                schema: "dbo",
                table: "PointTransaction",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationSchedule_DiseaseId",
                schema: "dbo",
                table: "VaccinationSchedule",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineBatch_VaccineId",
                schema: "dbo",
                table: "VaccineBatch",
                column: "VaccineId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineDisease_DiseaseId",
                schema: "dbo",
                table: "VaccineDisease",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineDisease_VaccineId",
                schema: "dbo",
                table: "VaccineDisease",
                column: "VaccineId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineExport_VaccineBatchId",
                schema: "dbo",
                table: "VaccineExport",
                column: "VaccineBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineExportDetail_AppointmentDetailId",
                schema: "dbo",
                table: "VaccineExportDetail",
                column: "AppointmentDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VaccineExportDetail_VaccineBatchId",
                schema: "dbo",
                table: "VaccineExportDetail",
                column: "VaccineBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineExportDetail_VaccineExportId",
                schema: "dbo",
                table: "VaccineExportDetail",
                column: "VaccineExportId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfile_DiseaseId",
                schema: "dbo",
                table: "VaccineProfile",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfile_PetId",
                schema: "dbo",
                table: "VaccineProfile",
                column: "PetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfile_VaccinationScheduleId",
                schema: "dbo",
                table: "VaccineProfile",
                column: "VaccinationScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineReceipt_VaccineBatchId",
                schema: "dbo",
                table: "VaccineReceipt",
                column: "VaccineBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineReceiptDetail_VaccineBatchId",
                schema: "dbo",
                table: "VaccineReceiptDetail",
                column: "VaccineBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineReceiptDetail_VaccineReceiptId",
                schema: "dbo",
                table: "VaccineReceiptDetail",
                column: "VaccineReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_Vet_AccountId",
                schema: "dbo",
                table: "Vet",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VetSchedule_VetId",
                schema: "dbo",
                table: "VetSchedule",
                column: "VetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PointTransaction",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VaccineDisease",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VaccineExportDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VaccineReceiptDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VetSchedule",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppointmentDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VaccineExport",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VaccineReceipt",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Appointment",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PetPassport",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ServiceHistory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VaccineProfile",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Vet",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VaccineBatch",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "HealthCondition",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MicrochipItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VaccinationSchedule",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Vaccine",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Microchip",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Pet",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Disease",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Account",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Membership",
                schema: "dbo");
        }
    }
}

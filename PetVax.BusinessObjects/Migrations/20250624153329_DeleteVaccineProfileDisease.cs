using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class DeleteVaccineProfileDisease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VaccineProfileDisease");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VaccineProfileDisease",
                columns: table => new
                {
                    VaccineProfileDiseasesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiseaseId = table.Column<int>(type: "int", nullable: true),
                    VaccineProfileId = table.Column<int>(type: "int", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineProfileDisease", x => x.VaccineProfileDiseasesId);
                    table.ForeignKey(
                        name: "FK_VaccineProfileDisease_Disease_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Disease",
                        principalColumn: "DiseaseId");
                    table.ForeignKey(
                        name: "FK_VaccineProfileDisease_VaccineProfile_VaccineProfileId",
                        column: x => x.VaccineProfileId,
                        principalTable: "VaccineProfile",
                        principalColumn: "VaccineProfileId");
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 24, 15, 25, 40, 925, DateTimeKind.Utc).AddTicks(4300), "yfDaWzxNt9PyOhTy2MKX0CuiYnk1c6/Napm5hCl6Dq4=", "jGhaOr0O2LyAFs27j+m6Xdzu005X1ZVY/dJ9SjNE4UM=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 24, 15, 25, 40, 925, DateTimeKind.Utc).AddTicks(4305), "AzepCFQwth70mpYieLLfv24IW4ogFGKSYCFTasrs8Tk=", "idlwWhPm4dPgnIko2hSk5QDoSRiPlFYd4tvptNM0k5Y=" });

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfileDisease_DiseaseId",
                table: "VaccineProfileDisease",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineProfileDisease_VaccineProfileId",
                table: "VaccineProfileDisease",
                column: "VaccineProfileId");
        }
    }
}

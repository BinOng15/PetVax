using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class CreateColdChainLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ColdChainLog",
                columns: table => new
                {
                    ColdChainLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VaccineBatchId = table.Column<int>(type: "int", nullable: false),
                    LogTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Humidity = table.Column<double>(type: "float", nullable: false),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColdChainLog", x => x.ColdChainLogId);
                    table.ForeignKey(
                        name: "FK_ColdChainLog_VaccineBatch_VaccineBatchId",
                        column: x => x.VaccineBatchId,
                        principalTable: "VaccineBatch",
                        principalColumn: "VaccineBatchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 14, 14, 26, 40, 696, DateTimeKind.Utc).AddTicks(4425), "HV79d4eeb/WSvIIN+Xzp5Y/TkbTJhkqbgCB7cwi1eIs=", "L5KdKrw8JFT3JzlhZkJaquInwaECVmgedhVAUe7Vb6Q=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 14, 14, 26, 40, 696, DateTimeKind.Utc).AddTicks(4430), "NUHJv3/dGUP5Qt0vG8nLva/O4vbq/dDxs3CCNOjCEtA=", "BWEDxAJPo8FJBONt/tJJe3BOHNrYtG8BLeDlYiLR/OQ=" });

            migrationBuilder.CreateIndex(
                name: "IX_ColdChainLog_VaccineBatchId",
                table: "ColdChainLog",
                column: "VaccineBatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColdChainLog");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 14, 14, 7, 57, 328, DateTimeKind.Utc).AddTicks(5898), "q+Bpi464Myl/C3It5spFviug3I9GZQ31V9wlsN98nh8=", "bbhKcrBuzFd7Gdci2ZKWylXRWgPGjxlsRGYkBS0TxiA=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 14, 14, 7, 57, 328, DateTimeKind.Utc).AddTicks(5909), "ZGqWYaNSpIlVdEQAj7nSJP194KDm966dFGcilKrV5Rw=", "z904tRcV+ZFr8Yru8+n/2L2e/VMy4T8Kc4PLxYcwj90=" });
        }
    }
}

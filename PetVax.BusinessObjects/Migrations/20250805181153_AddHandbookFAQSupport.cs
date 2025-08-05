using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class AddHandbookFAQSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "ServiceHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "FAQItem",
                columns: table => new
                {
                    FAQItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQItem", x => x.FAQItemId);
                });

            migrationBuilder.CreateTable(
                name: "Handbook",
                columns: table => new
                {
                    HandbookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Introduction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Highlight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImportantNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Handbook", x => x.HandbookId);
                });

            migrationBuilder.CreateTable(
                name: "SupportCategory",
                columns: table => new
                {
                    SupportCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportCategory", x => x.SupportCategoryId);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 5, 18, 11, 52, 378, DateTimeKind.Utc).AddTicks(4470), "/jVB/Uod6JAEqlallfXLo+1bhTIJYZeoy7VD/ohyHGE=", "vfcAu6oZoJhzrEpbZrCPlk9D4IId3Wdq7AOJ1srm9Ys=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 5, 18, 11, 52, 378, DateTimeKind.Utc).AddTicks(4479), "nXRB/EgbEq2i2tse76eOwmCRaQzubu1SRvL+mhUp1JQ=", "qarbrrtrWreYFt4ig8Wm8TgXq9XdBaKz2l8kbfdcuVY=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQItem");

            migrationBuilder.DropTable(
                name: "Handbook");

            migrationBuilder.DropTable(
                name: "SupportCategory");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethod",
                table: "ServiceHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class _123123123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 21, 29, 807, DateTimeKind.Utc).AddTicks(1281), "L36KsF0QL8NQeMxFAbjiBOgtfalpmADeLOZtGhApWnU=", "Wov2mD98Pxdh3SkY4uq4kaMggPlEwfe8NXVStNricnU=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 21, 29, 807, DateTimeKind.Utc).AddTicks(1287), "nqZ4Yk2BF1WU3XlDdRInOd93kMZwdzKcdQH5Vchcrhw=", "lC3PIvhNKnqdpDG6DSHKs9AlLfu/tYEGO127ZY2hqgw=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 16, 18, 606, DateTimeKind.Utc).AddTicks(3545), "OnIX6H8p7nN7drJqocSchPH+JtXtOtK0/tqhR5c3dJs=", "emWZCN7wwu/4WgIyeukUthmG3UxeeiHRpFZFZKPG8Lc=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 16, 18, 606, DateTimeKind.Utc).AddTicks(3548), "98Vh5C6MQmqql3am4ckvyvZu7Di1Nb0GLqUiH05EL1E=", "MOmRz5rb2a4dVCN6P7mTLp3aVxTJMyRczrOallIlGl8=" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStoragecondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StorageConditions",
                table: "VaccineBatch",
                newName: "StorageCondition");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 22, 13, 54, 16, 258, DateTimeKind.Utc).AddTicks(4410), "zPoW/OGbDARxfienu8kdldOXDMldQL0VmjdfHM/NZDE=", "dB4mBpUCimg/yu5NQ+bK7sOKe+ZH/l4SDg8cydCFVFs=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 22, 13, 54, 16, 258, DateTimeKind.Utc).AddTicks(4417), "m+RnX3OU06/D2Vxe725Gc83T2WYCNLjVSZDGalIB1to=", "6qBvxVBFgxF0SEqGRHbs5H3WDjcgyTR89pEVONifQec=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StorageCondition",
                table: "VaccineBatch",
                newName: "StorageConditions");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 19, 10, 7, 40, 480, DateTimeKind.Utc).AddTicks(852), "HoJzl9v+5jToIKzkP2TBbHOPXA7QByhUNgS0XEfPSqY=", "dZKalaCdwaetF/DzLbiUL/Bs2RLP0lH99oT30LPKpHE=" });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 8, 19, 10, 7, 40, 480, DateTimeKind.Utc).AddTicks(859), "vov7kgCApeJMM3C2b8aD6PEUDiXh13h9jw7cQhLl4I0=", "9vjFxsJS2Lj4NiGAf799oWRycOn2wD7oKokvm6DTjgo=" });
        }
    }
}

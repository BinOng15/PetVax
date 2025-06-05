using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVax.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class postgre_ver6 : Migration
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
                values: new object[] { new DateTime(2025, 6, 5, 8, 14, 10, 579, DateTimeKind.Utc).AddTicks(7208), "sCNBKJbd66bDPxgbZ5aCbIHVwmaMemJB7kvHuRMFDfs=", "UIf9h7+gBXgKWpIxZlK5WoFyCl2cmvXrm6FqEsiVbYI=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 5, 8, 14, 10, 579, DateTimeKind.Utc).AddTicks(7212), "ijHu+XZ3JQTVWSO4gOxKnIwpWFOdg3uEuSi+0QGXF9M=", "7T+af0OXeHYxWzPcc8B0qMZRUc7GVVUNAGvhWTp2sTs=" });
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
                values: new object[] { new DateTime(2025, 6, 5, 7, 48, 56, 301, DateTimeKind.Utc).AddTicks(2322), "F3paenpsVK+JcK16nL20HYQ6ecIxzd3iWReyEqCNOds=", "DMGagyXqsBxkvaqdA2GygFXKu0aFPcVCuwyX3+5f7VI=" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Account",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 5, 7, 48, 56, 301, DateTimeKind.Utc).AddTicks(2329), "bNDfE9sHBCrxyBUdxdAeeq7V9C3EqTQFuSRedOVTy6M=", "1ojf/bBP+TFBOac6IQgJiECzQ7eXkR1Nv5YlHZiYjFg=" });
        }
    }
}

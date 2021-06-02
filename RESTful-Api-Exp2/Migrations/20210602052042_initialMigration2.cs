using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RESTful_Api_Exp2.Migrations
{
    public partial class initialMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Companies",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("5ce682ef-bc20-4e11-bce6-12916576698e"),
                column: "StartTime",
                value: new DateTime(2021, 6, 2, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("c2e5433f-0b26-45ca-9d71-7c5e54af6617"),
                column: "StartTime",
                value: new DateTime(2021, 6, 2, 0, 20, 42, 19, DateTimeKind.Local).AddTicks(9164));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Companies",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("5ce682ef-bc20-4e11-bce6-12916576698e"),
                column: "StartTime",
                value: new DateTime(2021, 6, 1, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("c2e5433f-0b26-45ca-9d71-7c5e54af6617"),
                column: "StartTime",
                value: new DateTime(2021, 6, 1, 23, 58, 33, 130, DateTimeKind.Local).AddTicks(3406));
        }
    }
}

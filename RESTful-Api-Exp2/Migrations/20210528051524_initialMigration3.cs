using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RESTful_Api_Exp2.Migrations
{
    public partial class initialMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoFileName",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("5ce682ef-bc20-4e11-bce6-12916576698e"),
                column: "StartTime",
                value: new DateTime(2021, 5, 28, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("c2e5433f-0b26-45ca-9d71-7c5e54af6617"),
                column: "StartTime",
                value: new DateTime(2021, 5, 28, 0, 15, 23, 869, DateTimeKind.Local).AddTicks(2826));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("6c561b72-f44b-40ee-ba4b-a77d17aa8fef"),
                column: "PhotoFileName",
                value: "");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("a06b7e5d-83e7-473d-ad40-79d9fadfadc8"),
                column: "PhotoFileName",
                value: "");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"),
                column: "PhotoFileName",
                value: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoFileName",
                table: "Employees");

            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("5ce682ef-bc20-4e11-bce6-12916576698e"),
                column: "StartTime",
                value: new DateTime(2021, 5, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("c2e5433f-0b26-45ca-9d71-7c5e54af6617"),
                column: "StartTime",
                value: new DateTime(2021, 5, 21, 0, 7, 10, 773, DateTimeKind.Local).AddTicks(7215));
        }
    }
}

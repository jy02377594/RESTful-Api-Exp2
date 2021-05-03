using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RESTful_Api_Exp2.Migrations
{
    public partial class InitialMigration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTasks_Employees_EmployeeId",
                table: "EmployeeTasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "EmployeeTasks",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("5ce682ef-bc20-4e11-bce6-12916576698e"),
                column: "StartTime",
                value: new DateTime(2021, 5, 2, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("c2e5433f-0b26-45ca-9d71-7c5e54af6617"),
                column: "StartTime",
                value: new DateTime(2021, 5, 2, 11, 51, 39, 889, DateTimeKind.Local).AddTicks(3613));

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTasks_Employees_EmployeeId",
                table: "EmployeeTasks",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTasks_Employees_EmployeeId",
                table: "EmployeeTasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "EmployeeTasks",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("5ce682ef-bc20-4e11-bce6-12916576698e"),
                column: "StartTime",
                value: new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("c2e5433f-0b26-45ca-9d71-7c5e54af6617"),
                column: "StartTime",
                value: new DateTime(2021, 4, 29, 18, 42, 0, 945, DateTimeKind.Local).AddTicks(7256));

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTasks_Employees_EmployeeId",
                table: "EmployeeTasks",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

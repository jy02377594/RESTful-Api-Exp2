using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RESTful_Api_Exp2.Migrations
{
    public partial class initialMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("c2e5433f-0b26-45ca-9d71-7c5e54af6617"),
                column: "StartTime",
                value: new DateTime(2021, 5, 21, 0, 7, 10, 773, DateTimeKind.Local).AddTicks(7215));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("c2e5433f-0b26-45ca-9d71-7c5e54af6617"),
                column: "StartTime",
                value: new DateTime(2021, 5, 21, 0, 4, 30, 733, DateTimeKind.Local).AddTicks(7719));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RESTful_Api_Exp2.Migrations
{
    public partial class initialMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dateline",
                table: "EmployeeTasks",
                newName: "Deadline");

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("c2e5433f-0b26-45ca-9d71-7c5e54af6617"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"), new DateTime(2021, 4, 29, 18, 42, 0, 945, DateTimeKind.Local).AddTicks(7256), "fixxxxxxxxxxxxxxxxxxxxxxx buggggggggggggggggs", "fix bugs" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("05647a01-d14f-4893-b85b-e277e209ae52"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"), new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "test ttttttttttttttt", "test" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("5ce682ef-bc20-4e11-bce6-12916576698e"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a06b7e5d-83e7-473d-ad40-79d9fadfadc8"), new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Local), "make america great again", "bullshit" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("26cd2cef-928e-40a4-99e1-7fe7770be4ac"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"), new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "create a new table, query all task list, delete a row", "play" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("52b2279e-38a4-4d85-9d6c-3fa196ea3b24"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"), new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "I know your monster, I know your pain", "go home" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("05647a01-d14f-4893-b85b-e277e209ae52"));

            migrationBuilder.DeleteData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("26cd2cef-928e-40a4-99e1-7fe7770be4ac"));

            migrationBuilder.DeleteData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("52b2279e-38a4-4d85-9d6c-3fa196ea3b24"));

            migrationBuilder.DeleteData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("5ce682ef-bc20-4e11-bce6-12916576698e"));

            migrationBuilder.DeleteData(
                table: "EmployeeTasks",
                keyColumn: "taskId",
                keyValue: new Guid("c2e5433f-0b26-45ca-9d71-7c5e54af6617"));

            migrationBuilder.RenameColumn(
                name: "Deadline",
                table: "EmployeeTasks",
                newName: "Dateline");
        }
    }
}

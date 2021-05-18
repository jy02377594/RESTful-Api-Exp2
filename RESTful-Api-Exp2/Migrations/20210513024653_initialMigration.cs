using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RESTful_Api_Exp2.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Introduction = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmployeeNo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    HiredDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTasks",
                columns: table => new
                {
                    taskId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TaskName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    TaskDescription = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Deadline = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTasks", x => x.taskId);
                    table.ForeignKey(
                        name: "FK_EmployeeTasks_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda14"), "Good Company", "WeiRuan" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("6c561b72-f44b-40ee-ba4b-a77d17aa8fff"), "Bad Company", "PDD" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("a06b7e5d-83e7-473d-ad40-79d9fadfadb8"), "Fuck 996", "Alibaba" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("bf6ca130-51af-4e42-aee6-16a2b922c8b8"), "996 icu 007 death", "DreamCompany1" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("136fa82f-57f7-4479-90cc-8dc4198d0ec1"), "996 icu 007 death", "DreamCompany2" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("b80d95ba-30fc-4eb9-b138-7b841497b457"), "996 icu 007 death", "DreamCompany3" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("5f05d00d-11c5-4796-9f86-270ccd9560e0"), "996 icu 007 death", "DreamCompany4" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("d103dd72-ef31-45d5-a9ff-0b2bd17788a8"), "996 icu 007 death", "DreamCompany5" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "EmployeeNo", "FirstName", "Gender", "HiredDate", "LastName" },
                values: new object[] { new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"), new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda14"), "00001", "Xiaopeng", 1, new DateTime(2021, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luo" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "EmployeeNo", "FirstName", "Gender", "HiredDate", "LastName" },
                values: new object[] { new Guid("6c561b72-f44b-40ee-ba4b-a77d17aa8fef"), new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda14"), "00002", "Guanxi", 1, new DateTime(2015, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chen" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "EmployeeNo", "FirstName", "Gender", "HiredDate", "LastName" },
                values: new object[] { new Guid("a06b7e5d-83e7-473d-ad40-79d9fadfadc8"), new Guid("a06b7e5d-83e7-473d-ad40-79d9fadfadb8"), "00003", "Donald", 1, new DateTime(2021, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trump" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("c2e5433f-0b26-45ca-9d71-7c5e54af6617"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"), new DateTime(2021, 5, 12, 21, 46, 52, 867, DateTimeKind.Local).AddTicks(2735), "fixxxxxxxxxxxxxxxxxxxxxxx buggggggggggggggggs", "fix bugs" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("05647a01-d14f-4893-b85b-e277e209ae52"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"), new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "test ttttttttttttttt", "test" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("26cd2cef-928e-40a4-99e1-7fe7770be4ac"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"), new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "create a new table, query all task list, delete a row", "play" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("52b2279e-38a4-4d85-9d6c-3fa196ea3b24"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"), new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "I know your monster, I know your pain", "go home" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("9e0dd20e-659d-4f37-87be-9b24b7b69b98"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"), new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "I know your monster, I know your pain", "go home again" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("84bdba17-e4d6-4b2a-8ade-7d8606eff3ea"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"), new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "I know your monster, I know your pain", "go home4" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("747e7a32-3876-44ce-b85f-dd557b2984d9"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("6c561b72-f44b-40ee-ba4b-a77d17aa8fef"), new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "I know your monster, I know your pain", "go home2" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("d547664e-249d-4f88-b1db-dcfd0bf3d623"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("6c561b72-f44b-40ee-ba4b-a77d17aa8fef"), new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "I know your monster, I know your pain", "go home5" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("5ce682ef-bc20-4e11-bce6-12916576698e"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a06b7e5d-83e7-473d-ad40-79d9fadfadc8"), new DateTime(2021, 5, 12, 0, 0, 0, 0, DateTimeKind.Local), "make america great again", "bullshit" });

            migrationBuilder.InsertData(
                table: "EmployeeTasks",
                columns: new[] { "taskId", "Deadline", "EmployeeId", "StartTime", "TaskDescription", "TaskName" },
                values: new object[] { new Guid("98549e15-5596-4d43-9106-0bd2d67bf7c8"), new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a06b7e5d-83e7-473d-ad40-79d9fadfadc8"), new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "I know your monster, I know your pain", "go home3" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTasks_EmployeeId",
                table: "EmployeeTasks",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeTasks");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}

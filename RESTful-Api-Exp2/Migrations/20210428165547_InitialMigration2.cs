using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RESTful_Api_Exp2.Migrations
{
    public partial class InitialMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("6c561b72-f44b-40ee-ba4b-a77d17aa8fef"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("a06b7e5d-83e7-473d-ad40-79d9fadfadc8"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"));
        }
    }
}

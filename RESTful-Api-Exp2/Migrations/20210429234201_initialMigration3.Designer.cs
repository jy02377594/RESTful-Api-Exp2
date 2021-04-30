﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RESTful_Api_Exp2.Data;

namespace RESTful_Api_Exp2.Migrations
{
    [DbContext(typeof(Restful_DbContext))]
    [Migration("20210429234201_initialMigration3")]
    partial class initialMigration3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("RESTful_Api_Exp2.Entities.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Introduction")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda14"),
                            Introduction = "Good Company",
                            Name = "WeiRuan"
                        },
                        new
                        {
                            Id = new Guid("6c561b72-f44b-40ee-ba4b-a77d17aa8fff"),
                            Introduction = "Bad Company",
                            Name = "PDD"
                        },
                        new
                        {
                            Id = new Guid("a06b7e5d-83e7-473d-ad40-79d9fadfadb8"),
                            Introduction = "Fuck 996",
                            Name = "Alibaba"
                        });
                });

            modelBuilder.Entity("RESTful_Api_Exp2.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmployeeNo")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("HiredDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"),
                            CompanyId = new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda14"),
                            EmployeeNo = "00001",
                            FirstName = "Xiaopeng",
                            Gender = 1,
                            HiredDate = new DateTime(2021, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Luo"
                        },
                        new
                        {
                            Id = new Guid("6c561b72-f44b-40ee-ba4b-a77d17aa8fef"),
                            CompanyId = new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda14"),
                            EmployeeNo = "00002",
                            FirstName = "Guanxi",
                            Gender = 1,
                            HiredDate = new DateTime(2015, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Chen"
                        },
                        new
                        {
                            Id = new Guid("a06b7e5d-83e7-473d-ad40-79d9fadfadc8"),
                            CompanyId = new Guid("a06b7e5d-83e7-473d-ad40-79d9fadfadb8"),
                            EmployeeNo = "00003",
                            FirstName = "Donald",
                            Gender = 1,
                            HiredDate = new DateTime(2021, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Trump"
                        });
                });

            modelBuilder.Entity("RESTful_Api_Exp2.Entities.EmployeeTask", b =>
                {
                    b.Property<Guid>("taskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("TaskDescription")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.HasKey("taskId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeTasks");

                    b.HasData(
                        new
                        {
                            taskId = new Guid("c2e5433f-0b26-45ca-9d71-7c5e54af6617"),
                            Deadline = new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeId = new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"),
                            StartTime = new DateTime(2021, 4, 29, 18, 42, 0, 945, DateTimeKind.Local).AddTicks(7256),
                            TaskDescription = "fixxxxxxxxxxxxxxxxxxxxxxx buggggggggggggggggs",
                            TaskName = "fix bugs"
                        },
                        new
                        {
                            taskId = new Guid("05647a01-d14f-4893-b85b-e277e209ae52"),
                            Deadline = new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeId = new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"),
                            StartTime = new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TaskDescription = "test ttttttttttttttt",
                            TaskName = "test"
                        },
                        new
                        {
                            taskId = new Guid("5ce682ef-bc20-4e11-bce6-12916576698e"),
                            Deadline = new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeId = new Guid("a06b7e5d-83e7-473d-ad40-79d9fadfadc8"),
                            StartTime = new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Local),
                            TaskDescription = "make america great again",
                            TaskName = "bullshit"
                        },
                        new
                        {
                            taskId = new Guid("26cd2cef-928e-40a4-99e1-7fe7770be4ac"),
                            Deadline = new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeId = new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"),
                            StartTime = new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TaskDescription = "create a new table, query all task list, delete a row",
                            TaskName = "play"
                        },
                        new
                        {
                            taskId = new Guid("52b2279e-38a4-4d85-9d6c-3fa196ea3b24"),
                            Deadline = new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeId = new Guid("a3a461ea-e692-6f54-2f3e-f076a08dda15"),
                            StartTime = new DateTime(2021, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TaskDescription = "I know your monster, I know your pain",
                            TaskName = "go home"
                        });
                });

            modelBuilder.Entity("RESTful_Api_Exp2.Entities.Employee", b =>
                {
                    b.HasOne("RESTful_Api_Exp2.Entities.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("RESTful_Api_Exp2.Entities.EmployeeTask", b =>
                {
                    b.HasOne("RESTful_Api_Exp2.Entities.Employee", "Employees")
                        .WithMany("Tasklist")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Employees");
                });

            modelBuilder.Entity("RESTful_Api_Exp2.Entities.Company", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("RESTful_Api_Exp2.Entities.Employee", b =>
                {
                    b.Navigation("Tasklist");
                });
#pragma warning restore 612, 618
        }
    }
}

using Microsoft.EntityFrameworkCore;
using RESTful_Api_Exp2.Entities;
using System;

namespace RESTful_Api_Exp2.Data
{
    //inherit Dbcontext class from entityframework core
    public class Restful_DbContext : DbContext
    {
        // constructor
        public Restful_DbContext(DbContextOptions<Restful_DbContext> options) : base(options)  //调用父类构造函数把option传进去
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeTask> EmployeeTasks { get; set; }
        public object Tasks { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Company>().Property(x => x.Introduction).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<Company>().Property(x => x.Country).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Company>().Property(x => x.Industry).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Company>().Property(x => x.Product).IsRequired().HasMaxLength(100);

        modelBuilder.Entity<Employee>().Property(x => x.EmployeeNo).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Employee>().Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(x => x.LastName).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<EmployeeTask>().Property(x => x.TaskName).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<EmployeeTask>().Property(x => x.TaskDescription).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<EmployeeTask>().HasKey(x => x.taskId);

            modelBuilder.Entity<Employee>()
                .HasOne(navigationExpression: x => x.Company)
                .WithMany(navigationExpression: x => x.Employees)
                .HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);

            //如果要建立多对多关系这里要写多次HasOne, WithMany
            //这里的业务需求要让外键为空
            modelBuilder.Entity<EmployeeTask>()
                .HasOne(navigationExpression: x => x.Employees)
                .WithMany(navigationExpression: x => x.Tasklist)
                .HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = Guid.Parse("a3a461ea-e692-6f54-2f3e-f076a08dda14"),
                    Name = "WeiRuan",
                    Introduction = "Good Company",
                    Country = "USA",
                    Industry = "Software",
                    Product = "VisualStudio"
                },
                new Company
                {
                    Id = Guid.Parse("6c561b72-f44b-40ee-ba4b-a77d17aa8fff"),
                    Name = "PDD",
                    Introduction = "Bad Company",
                    Country = "China",
                    Industry = "e-commerce",
                    Product = "Kan yi dao"
                },
                new Company
                {
                    Id = Guid.Parse("a06b7e5d-83e7-473d-ad40-79d9fadfadb8"),
                    Name = "Alibaba",
                    Introduction = "Fuck 996",
                    Country = "China",
                    Industry = "e-commerce",
                    Product = "996 ICU"
                },
                new Company
                {
                    Id = Guid.Parse("bf6ca130-51af-4e42-aee6-16a2b922c8b8"),
                    Name = "DreamCompany1",
                    Introduction = "996 icu 007 death",
                    Country = "China",
                    Industry = "e-commerce",
                    Product = "996 ICU"
                },
                new Company
                {
                    Id = Guid.Parse("136fa82f-57f7-4479-90cc-8dc4198d0ec1"),
                    Name = "DreamCompany2",
                    Introduction = "996 icu 007 death",
                    Country = "China",
                    Industry = "e-commerce",
                    Product = "996 ICU"
                },
                new Company
                {
                    Id = Guid.Parse("b80d95ba-30fc-4eb9-b138-7b841497b457"),
                    Name = "DreamCompany3",
                    Introduction = "996 icu 007 death",
                    Country = "China",
                    Industry = "e-commerce",
                    Product = "996 ICU"
                },
                new Company
                {
                    Id = Guid.Parse("5f05d00d-11c5-4796-9f86-270ccd9560e0"),
                    Name = "DreamCompany4",
                    Introduction = "996 icu 007 death",
                    Country = "China",
                    Industry = "e-commerce",
                    Product = "996 ICU"
                },
                new Company
                {
                    Id = Guid.Parse("d103dd72-ef31-45d5-a9ff-0b2bd17788a8"),
                    Name = "DreamCompany5",
                    Introduction = "996 icu 007 death",
                    Country = "China",
                    Industry = "e-commerce",
                    Product = "996 ICU"
                }
                );

            modelBuilder.Entity<Employee>().HasData(
               new Employee
               {
                   Id = Guid.Parse("a3a461ea-e692-6f54-2f3e-f076a08dda15"),
                   CompanyId = Guid.Parse("a3a461ea-e692-6f54-2f3e-f076a08dda14"),
                   EmployeeNo = "00001",
                   FirstName = "Xiaopeng",
                   LastName = "Luo",
                   Gender = Gender.male,
                   HiredDate = DateTime.Parse("2021-4-28"),
               },
               new Employee
               {
                   Id = Guid.Parse("6c561b72-f44b-40ee-ba4b-a77d17aa8fef"),
                   CompanyId = Guid.Parse("a3a461ea-e692-6f54-2f3e-f076a08dda14"),
                   EmployeeNo = "00002",
                   FirstName = "Guanxi",
                   LastName = "Chen",
                   Gender = Gender.male,
                   HiredDate = DateTime.Parse("2015-3-23"),
               },
               new Employee
               {
                   Id = Guid.Parse("a06b7e5d-83e7-473d-ad40-79d9fadfadc8"),
                   CompanyId = Guid.Parse("a06b7e5d-83e7-473d-ad40-79d9fadfadb8"),
                   EmployeeNo = "00003",
                   FirstName = "Donald",
                   LastName = "Trump",
                   Gender = Gender.male,
                   HiredDate = DateTime.Parse("2021-4-28"),
               }
               );

            modelBuilder.Entity<EmployeeTask>().HasData(
                new EmployeeTask
                {
                    taskId = Guid.Parse("c2e5433f-0b26-45ca-9d71-7c5e54af6617"),
                    EmployeeId = Guid.Parse("a3a461ea-e692-6f54-2f3e-f076a08dda15"),
                    TaskName = "fix bugs",
                    TaskDescription = "fixxxxxxxxxxxxxxxxxxxxxxx buggggggggggggggggs",
                    StartTime = DateTime.Now,
                    Deadline = DateTime.Parse("2021/5/15")
                },
                new EmployeeTask
                {
                    taskId = Guid.Parse("05647a01-d14f-4893-b85b-e277e209ae52"),
                    EmployeeId = Guid.Parse("a3a461ea-e692-6f54-2f3e-f076a08dda15"),
                    TaskName = "test",
                    TaskDescription = "test ttttttttttttttt",
                    StartTime = DateTime.Parse("2021/4/29"),
                    Deadline = DateTime.Parse("2021/5/15")
                },
                new EmployeeTask
                {
                    taskId = Guid.Parse("5ce682ef-bc20-4e11-bce6-12916576698e"),
                    EmployeeId = Guid.Parse("a06b7e5d-83e7-473d-ad40-79d9fadfadc8"),
                    TaskName = "bullshit",
                    TaskDescription = "make america great again",
                    StartTime = DateTime.Today,
                    Deadline = DateTime.Parse("2021/5/15")
                },
                new EmployeeTask
                {
                    taskId = Guid.Parse("26cd2cef-928e-40a4-99e1-7fe7770be4ac"),
                    EmployeeId = Guid.Parse("a3a461ea-e692-6f54-2f3e-f076a08dda15"),
                    TaskName = "play",
                    TaskDescription = "create a new table, query all task list, delete a row",
                    StartTime = DateTime.Parse("2021/4/29"),
                    Deadline = DateTime.Parse("2021/5/15")
                },
                new EmployeeTask
                {
                    taskId = Guid.Parse("52b2279e-38a4-4d85-9d6c-3fa196ea3b24"),
                    EmployeeId = Guid.Parse("a3a461ea-e692-6f54-2f3e-f076a08dda15"),
                    TaskName = "go home",
                    TaskDescription = "I know your monster, I know your pain",
                    StartTime = DateTime.Parse("2021/4/29"),
                    Deadline = DateTime.Parse("2021/5/15")
                },
                                new EmployeeTask
                                {
                                    taskId = Guid.Parse("9e0dd20e-659d-4f37-87be-9b24b7b69b98"),
                                    EmployeeId = Guid.Parse("a3a461ea-e692-6f54-2f3e-f076a08dda15"),
                                    TaskName = "go home again",
                                    TaskDescription = "I know your monster, I know your pain",
                                    StartTime = DateTime.Parse("2021/4/29"),
                                    Deadline = DateTime.Parse("2021/5/15")
                                },
                                                new EmployeeTask
                                                {
                                                    taskId = Guid.Parse("747e7a32-3876-44ce-b85f-dd557b2984d9"),
                                                    EmployeeId = Guid.Parse("6c561b72-f44b-40ee-ba4b-a77d17aa8fef"),
                                                    TaskName = "go home2",
                                                    TaskDescription = "I know your monster, I know your pain",
                                                    StartTime = DateTime.Parse("2021/4/29"),
                                                    Deadline = DateTime.Parse("2021/5/15")
                                                },
                                                                new EmployeeTask
                                                                {
                                                                    taskId = Guid.Parse("98549e15-5596-4d43-9106-0bd2d67bf7c8"),
                                                                    EmployeeId = Guid.Parse("a06b7e5d-83e7-473d-ad40-79d9fadfadc8"),
                                                                    TaskName = "go home3",
                                                                    TaskDescription = "I know your monster, I know your pain",
                                                                    StartTime = DateTime.Parse("2021/4/29"),
                                                                    Deadline = DateTime.Parse("2021/5/15")
                                                                },
                                                                                new EmployeeTask
                                                                                {
                                                                                    taskId = Guid.Parse("84bdba17-e4d6-4b2a-8ade-7d8606eff3ea"),
                                                                                    EmployeeId = Guid.Parse("a3a461ea-e692-6f54-2f3e-f076a08dda15"),
                                                                                    TaskName = "go home4",
                                                                                    TaskDescription = "I know your monster, I know your pain",
                                                                                    StartTime = DateTime.Parse("2021/4/29"),
                                                                                    Deadline = DateTime.Parse("2021/5/15")
                                                                                },
                                                                                                new EmployeeTask
                                                                                                {
                                                                                                    taskId = Guid.Parse("d547664e-249d-4f88-b1db-dcfd0bf3d623"),
                                                                                                    EmployeeId = Guid.Parse("6c561b72-f44b-40ee-ba4b-a77d17aa8fef"),
                                                                                                    TaskName = "go home5",
                                                                                                    TaskDescription = "I know your monster, I know your pain",
                                                                                                    StartTime = DateTime.Parse("2021/4/29"),
                                                                                                    Deadline = DateTime.Parse("2021/5/15")
                                                                                                }
            );
        }
    }
}

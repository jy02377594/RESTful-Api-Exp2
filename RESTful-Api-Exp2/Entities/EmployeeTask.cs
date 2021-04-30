using System;
using System.ComponentModel.DataAnnotations;

namespace RESTful_Api_Exp2.Entities
{
    public class EmployeeTask
    {
        [Key]
        public Guid taskId { get; set; } //pk
        public Guid EmployeeId { get; set; } // foreign key
        public string TaskName { get;set; }
        public string TaskDescription { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Deadline { get; set; }
        public Employee Employees { get; set; }
    }
}
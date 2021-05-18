using System;
using System.ComponentModel.DataAnnotations;

namespace RESTful_Api_Exp2.Entities
{
    public class EmployeeTask
    {
        [Key]
        public Guid taskId { get; set; } //pk
        public Guid? EmployeeId { get; set; }
        public string TaskName { get;set; }
        public string TaskDescription { get; set; }
        [DataType(DataType.Date, ErrorMessage = "The Start Time Must be a Date Type")]
        public DateTime StartTime { get; set; }
        [DataType(DataType.Date, ErrorMessage = "The Deadline Must be a Date Type")]
        public DateTime Deadline { get; set; }
        public Employee Employees { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Models
{
    public class EmployeeTaskDto
    {
        public Guid taskId { get; set; }
        public Guid EmployeeId { get; set; } // foreign key
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Deadline { get; set; }
        public int taskDays { get; set; }
       // public EmployeeDto Employees { get; set; }
    }
}

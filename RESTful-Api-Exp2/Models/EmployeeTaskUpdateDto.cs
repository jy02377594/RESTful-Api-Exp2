using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Models
{
    public class EmployeeTaskUpdateDto
    {
        public Guid? EmployeeId { get; set; }

        [MaxLength(50, ErrorMessage = "The length of {0} must be less than 50")]
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Deadline { get; set; }
    }
}

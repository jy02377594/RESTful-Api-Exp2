using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Models
{
    public class EmployeeDto
    {
        public Guid Id { get; set; } // primary key
        public Guid CompanyId { get; set; } // foreign key, point to company entity
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string GenderDisplay { get; set; }
        public int WorkAge { get; set; }
    }
}

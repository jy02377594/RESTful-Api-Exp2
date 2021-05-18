using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Entities
{
    public class Employee
    {
        public Guid Id { get; set; } // primary key
        public Guid CompanyId { get; set; } // foreign key, point to company entity
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        [DataType(DataType.Date, ErrorMessage = "The HiredDate Must be a Date Type")]
        public DateTime HiredDate { get; set; }
        public ICollection<EmployeeTask> Tasklist { get; set; }
        public Company Company { get; set; } //To build relationship between two tables
    }
}

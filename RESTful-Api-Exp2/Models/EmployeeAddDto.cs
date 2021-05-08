using RESTful_Api_Exp2.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Models
{
    public class EmployeeAddDto
    {
        [Required(ErrorMessage = "The {0} is required")]
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime HiredDate { get; set; }
        public ICollection<EmployeeTaskAddDto> Tasklist { get; set; } = new List<EmployeeTaskAddDto>();//不new的话添加不带task的新employee会引起空异常
    }
}

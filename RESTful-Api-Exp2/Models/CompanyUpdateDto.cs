using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Models
{
    public class CompanyUpdateDto: CompanyAddOrUpdateDto
    {
        [MaxLength(50, ErrorMessage = "The length of {0} can not beyond 50")]
        public override string Introduction { get; set; }
    }
}

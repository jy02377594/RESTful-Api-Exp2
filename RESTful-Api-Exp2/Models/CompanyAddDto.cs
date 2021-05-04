using RESTful_Api_Exp2.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Models
{

    public class CompanyAddDto: CompanyAddOrUpdateDto
    {
       [Required(ErrorMessage = "The {0} filed is necessary")]
       public override string Introduction { get; set; }
    }
}

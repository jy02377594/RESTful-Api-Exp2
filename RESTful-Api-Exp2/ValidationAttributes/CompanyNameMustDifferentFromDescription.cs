using RESTful_Api_Exp2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.ValidationAttributes
{
    public class CompanyNameMustDifferentFromDescription: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var addDto = (CompanyAddOrUpdateDto)validationContext.ObjectInstance;

            if (addDto.Name == addDto.Introduction)
            {
                return new ValidationResult("Company name can not be same as Introduction", new[] { nameof(CompanyAddOrUpdateDto) });
            }

            return ValidationResult.Success;
        }
    }
}

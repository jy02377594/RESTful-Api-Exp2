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
            //批量传输时validationContext没法转换，这里批量做验证
            if (validationContext.ObjectType.Name.Contains("List"))
            {
                var addListDto = (List<CompanyAddDto>)validationContext.ObjectInstance;
                foreach (var Dto in addListDto)
                {
                    var addDto = (CompanyAddOrUpdateDto)Dto;
                    if (addDto.Name == addDto.Introduction)
                    {
                        return new ValidationResult("Company name can not be same as Introduction", new[] { nameof(CompanyAddOrUpdateDto) });
                    }
                }
            }
            else
            {
                var addDto = (CompanyAddOrUpdateDto)validationContext.ObjectInstance;
                if (addDto.Name == addDto.Introduction)
                {
                    return new ValidationResult("Company name can not be same as Introduction", new[] { nameof(CompanyAddOrUpdateDto) });
                }
            }
            return ValidationResult.Success;
        }
    }
}

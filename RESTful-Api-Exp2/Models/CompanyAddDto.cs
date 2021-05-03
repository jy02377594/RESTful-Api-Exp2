using RESTful_Api_Exp2.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Models
{
    //自定义Attribute 验证， 可以作用于类级别和属性级别
    [CompanyNameMustDifferentFromDescription]
    //为什么有CompnayDto的情况下还要增加不同的Dto?
    //虽然有的字段是重复的，但是在未来可能需求变更的情况下，创建，查询，更新所需要的Dto是不一样的，所以针对每项功能都创建新的Dto有助于扩展，重构
    public class CompanyAddDto: IValidatableObject
    {
        [Display(Name = "Company")]
        [Required(ErrorMessage = "The {0} field is necessary")]
        [MaxLength(100, ErrorMessage = "{0}'s Max Length can not beyond {1}")]
        public string Name { get; set; }
        [StringLength(500, MinimumLength = 10, ErrorMessage = "{0}'s Length Range is 10 to 500")]
        public string Introduction { get; set; }

        public ICollection<EmployeeAddDto> Employees { get; set; } = new List<EmployeeAddDto>();//new一下避免空引用异常

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //自定义验证规则
            if (Name == Introduction) yield return new ValidationResult("Name and Introduction can not be same", new[] {nameof(Name), nameof(Introduction)});
        }
    }
}

using RESTful_Api_Exp2.Entities;
using RESTful_Api_Exp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        //OrdinalIgnoreCase做字符比较忽略大小写
        private readonly Dictionary<string, PropertyMappingValue> _employeePropertyMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            //前面的字符串是Dto里的，后面的是entity里的
            { "Id", new PropertyMappingValue(new List<string>{"Id"})},
            { "CompanyId", new PropertyMappingValue(new List<string>{"CompanyId"})},
            { "EmployeeNo", new PropertyMappingValue(new List<string>{"EmployeeNo"})},
            { "EmployeeName", new PropertyMappingValue(new List<string>{"FirstName", "LastName"})},
            { "GenderDisplay", new PropertyMappingValue(new List<string>{"Gender"})},
            { "WorkAge", new PropertyMappingValue(new List<string>{"HiredDate"},revert:true)}
        };
        //  private IList<PropertyMapping<TSource, TDestination>> propertyMappings; 用接口代替这种写法
        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<EmployeeDto, Employee>(_employeePropertyMapping));
        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            //通过类型取出来
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            var propertyMappings = matchingMapping.ToList();
            if (propertyMappings.Count() == 1)
            {
                return propertyMappings.First().MappingDictionary;
            }

            throw new Exception($"can not find the unique mapping relationship:{ typeof(TSource)},{ typeof(TDestination)}");
        }
    }
}

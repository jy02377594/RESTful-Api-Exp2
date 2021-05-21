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
        //companymapping service，mapping的属性值
        private readonly Dictionary<string, PropertyMappingValue> _companyPropertyMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            //前面的字符串是Dto里的，后面的是entity里的
            { "Id", new PropertyMappingValue(new List<string>{"Id"})},
            { "Name", new PropertyMappingValue(new List<string>{"Name"})},
            { "Country", new PropertyMappingValue(new List<string>{"Country"})},
            { "Industry", new PropertyMappingValue(new List<string>{"Industry"})},
            { "Product", new PropertyMappingValue(new List<string>{"Product"})},
            { "Introduction", new PropertyMappingValue(new List<string>{"Introduction"})}
        };

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

        //构造函数，new初始化对象活得对象的属性，创建对象PropertyMappingService时完成
        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<EmployeeDto, Employee>(_employeePropertyMapping));
            _propertyMappings.Add(new PropertyMapping<CompanyDto, Company>(_companyPropertyMapping));
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

        //orderBy里的参数是否存在于companyDto
        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();
            if (string.IsNullOrWhiteSpace(fields)) return true;

            var fieldAfterSplit = fields.Split(",");
            foreach (var field in fieldAfterSplit)
            {
                var trimmedField = field.Trim();
                var indexOfFirstSpace = trimmedField.IndexOf(" ", StringComparison.Ordinal);
                var propertyName = indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace);

                //orderBy如有不存在于dto里的属性返回否
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }


            return true;
        }
    }
}

using RESTful_Api_Exp2.Entities;
using RESTful_Api_Exp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Services
{
    public class PropertyMappingServiceForTask:IPropertyMappingServiceForTask
    {
        //mapping属性值,OrdinalIgnoreCase忽略大小写
        private readonly Dictionary<string, PropertyMappingValueForTask> _employeeTaskPropertyMapping =
            new Dictionary<string, PropertyMappingValueForTask>(StringComparer.OrdinalIgnoreCase)
            {
                //EmoloyeetaskDto to EmployeeTask
                { "taskId", new PropertyMappingValueForTask(new List<string>{ "taskId"}, false)},
                { "EmployeeId", new PropertyMappingValueForTask(new List<string>{ "EmployeeId"}, false)},
                { "TaskName", new PropertyMappingValueForTask(new List<string>{ "TaskName"}, false)},
                { "TaskDescription", new PropertyMappingValueForTask(new List<string>{ "TaskDescription"}, false)},
                { "StartTime", new PropertyMappingValueForTask(new List<string>{ "StartTime"}, false)},
                { "Deadline", new PropertyMappingValueForTask(new List<string>{ "Deadline"}, false)},
                { "taskDays", new PropertyMappingValueForTask(new List<string>{ "StartTime","Deadline"}, false)},
            };
        //employee mapping
        private readonly Dictionary<string, PropertyMappingValueForTask> _employeePropertyMapping = new Dictionary<string, PropertyMappingValueForTask>(StringComparer.OrdinalIgnoreCase)
        {
            //前面的字符串是Dto里的，后面的是entity里的
            { "Id", new PropertyMappingValueForTask(new List<string>{"Id"},false)},
            { "CompanyId", new PropertyMappingValueForTask(new List<string>{"CompanyId"},false)},
            { "EmployeeNo", new PropertyMappingValueForTask(new List<string>{"EmployeeNo"},false)},
            { "EmployeeName", new PropertyMappingValueForTask(new List<string>{"FirstName", "LastName"},false)},
            { "GenderDisplay", new PropertyMappingValueForTask(new List<string>{"Gender"},false)},
            { "WorkAge", new PropertyMappingValueForTask(new List<string>{"HiredDate"},true)}
        };

        //这里list里必须是接口类型，因为下面要new一个PropertyMappingForTask
        private readonly IList<IPropertyMappingForTask> _propertyMappings = new List<IPropertyMappingForTask>();

        public PropertyMappingServiceForTask()
        {
            _propertyMappings.Add(new PropertyMappingForTask<EmployeeTaskDto, EmployeeTask>(_employeeTaskPropertyMapping));
            _propertyMappings.Add(new PropertyMappingForTask<EmployeeDto, Employee>(_employeePropertyMapping));
        }
        public Dictionary<string, PropertyMappingValueForTask> GetPropertyMapping<TSource, TDestination>()
        {
            var propertyMapping = _propertyMappings.OfType<PropertyMappingForTask<TSource, TDestination>>().ToList();
            //对应上了唯一property映射就正常返回
            if (propertyMapping.Count == 1)
            {
                return propertyMapping.First().MappingDictionary;
            }

            throw new Exception($"there is no unique mapping relationship:{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ExistsMapping<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();
            //如果排序的字符串是空，就不去做映射了，直接返回真，上一层返回默认排序的值
            if (string.IsNullOrWhiteSpace(fields)) return true;
            //对fields做一些字符串处理
            var fieldAfterSplit = fields.Split(",");
            foreach (var field in fieldAfterSplit)
            {
                var trimedField = field.Trim();
                //去空格所在的索引
                var indexOfFirstSpace = trimedField.IndexOf(" ");
                //有空格就去掉空格后面所有的字符串
                var propertyName = indexOfFirstSpace == -1 ? trimedField : trimedField.Remove(indexOfFirstSpace);
                //映射字典里没有对应的排序参数就返回false
                if (!propertyMapping.ContainsKey(propertyName)) return false;
            }


            return true;
        }
    }
}

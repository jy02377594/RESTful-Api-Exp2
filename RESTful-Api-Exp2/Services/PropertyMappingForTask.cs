using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Services
{
    //这个类的返回类型不知道，所以先用泛型占位符定义
    public class PropertyMappingForTask<TSource, TDestination>:IPropertyMappingForTask
    {
       public Dictionary<string, PropertyMappingValueForTask> MappingDictionary { get; set; }

        public PropertyMappingForTask(Dictionary<string, PropertyMappingValueForTask> mappingDictionary) {
            MappingDictionary = mappingDictionary ?? throw new ArgumentNullException(nameof(mappingDictionary));
        }
    }
}

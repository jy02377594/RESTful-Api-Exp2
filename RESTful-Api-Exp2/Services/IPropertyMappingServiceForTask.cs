using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Services
{
    public interface IPropertyMappingServiceForTask
    {
        Dictionary<string, PropertyMappingValueForTask> GetPropertyMapping<TSource, TDestination>();
        bool ExistsMapping<TSource, TDestination>(string fields);
    }
}

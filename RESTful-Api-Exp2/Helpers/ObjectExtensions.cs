using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Helpers
{
    //为什么写一个类似IEnumerableExtesions的扩展类？ 因为这里每个对象都需要反射，性能不同
    public static class ObjectExtensions
    {
        public static ExpandoObject ShapeData<TSource>(this TSource source, string fields)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var expandObj = new ExpandoObject();

            if (string.IsNullOrWhiteSpace(fields))
            {
                //反射出所有的属性
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)expandObj).Add(propertyInfo.Name, propertyValue);
                }
            }

            else
            {
                var fieldsAfterSplit = fields.Split(",");
                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();
                    //根据属性名反射一个属性信息
                    var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null) throw new Exception($"do not find {propertyName} on the {typeof(TSource)}");
                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)expandObj).Add(propertyInfo.Name, propertyValue);
                }
            }
            return expandObj;
        }
    }
}

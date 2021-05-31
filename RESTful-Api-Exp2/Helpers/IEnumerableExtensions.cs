using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Helpers
{
    //静态类静态方法才能组成扩展方法
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>
            (this IEnumerable<TSource> source, string fields)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var expandoObjectList = new List<ExpandoObject>(source.Count());
            //这里集合里的PropertyInfo提前根据类型提取出来了，后面遍历不需要反射了,PropertyInfo就是反射的属性信息
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(",");
                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if(propertyInfo == null) throw new Exception($"Property:do not find {propertyName}:{ typeof(TSource)}");
                    propertyInfoList.Add(propertyInfo);
                }
            }
            //propertyInfoList里存的需要的属性值，比如companyDto里只展示companyName，或者全部.
            //循环数据加到expandoObjectList
            foreach (TSource obj in source)
            {
                var shapedObj = new ExpandoObject();
                //这里就不需要反射了，属性信息在创建集合的时候就提取出来了
                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(obj);
                    ((IDictionary<string, object>)shapedObj).Add(propertyInfo.Name, propertyValue);
                }
                expandoObjectList.Add(shapedObj);
            }

            return expandoObjectList;
        }
    }
}

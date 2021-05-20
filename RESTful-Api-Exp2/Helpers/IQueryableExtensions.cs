using RESTful_Api_Exp2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Helpers
{
    public static class IQueryableExtensions
    {
        //根据传进来的字符串进行排序，而不是lambda表达式了
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (mappingDictionary == null) throw new ArgumentNullException(nameof(mappingDictionary));
            //如果orderBy为空就不排序，直接返回source
            if (string.IsNullOrWhiteSpace(orderBy)) return source;
            //根据逗号对排序参数进行分割一下
            var orderByAfterSplit = orderBy.Split(",");

            //这里为什么要反转?
            foreach (var orderByCaluse in orderByAfterSplit.Reverse())
            {
                var trimmedOrderByClasue = orderByCaluse.Trim();
                //判断是否倒叙
                var orderDescending = trimmedOrderByClasue.EndsWith(" desc");
                //判断字符串有没有空格
                var indexOfFirstSpace = trimmedOrderByClasue.IndexOf(" ");
                //根据空格情况返回属性名,有空格把空格后面的内容去掉
                var propertyName = indexOfFirstSpace == -1 ? trimmedOrderByClasue : trimmedOrderByClasue.Remove(indexOfFirstSpace);
                if (!mappingDictionary.ContainsKey(propertyName)) throw new ArgumentNullException($"do not find Key is {propertyName}'s mapping");

                var propertyMappingValue = mappingDictionary[propertyName];
                if (propertyMappingValue == null) throw new ArgumentNullException(nameof(propertyMappingValue));

                //为什么反转？
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }
                    source = source.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));
                }
            }

            return source;
        }
    }
}

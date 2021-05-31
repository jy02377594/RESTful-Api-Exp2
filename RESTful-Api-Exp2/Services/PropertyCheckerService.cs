using System.Reflection;

namespace RESTful_Api_Exp2.Services
{
    //这个服务只用来判断数据塑性里给的参数有没有对应的DTO
    public class PropertyCheckerService : IPropertyCheckerService
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields)) return true;

            var fieldsAfterSplit = fields.Split(",");
            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);

                if (propertyInfo == null) return false;
            }
            return true;
        }
    }
}

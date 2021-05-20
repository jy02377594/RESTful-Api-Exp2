using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Services
{
    public class PropertyMappingValue
    {
        //举例，dto里name属性对应的entity里firstname + lastname
        public IEnumerable<string> DestinationProperties { get; set; }
        //age和dateofbirth是负相关的，age越大,date越小
        public bool Revert { get; set; }

        //通常情况下是不翻转的，revert是为false
        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
        {
            DestinationProperties = destinationProperties ?? throw new ArgumentNullException(nameof(destinationProperties));
            Revert = revert;
        }
    }
}

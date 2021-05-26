using System;
using System.Collections.Generic;

namespace RESTful_Api_Exp2.Services
{
    public class PropertyMappingValueForTask
    {
        public IEnumerable<string> DestinationProperties { get; set; }
        public bool Revert { get; set; } = false;
        public PropertyMappingValueForTask(IEnumerable<string> destinationProperties, bool revert) {
            DestinationProperties = destinationProperties ?? throw new ArgumentNullException(nameof(destinationProperties));
            Revert = revert;
        }
    }
}
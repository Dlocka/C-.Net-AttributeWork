using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyExhNameAttribute:Attribute
    {
        public PropertyExhNameAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }
    }
}

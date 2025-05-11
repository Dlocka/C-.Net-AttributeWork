using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [AttributeUsage(AttributeTargets.Field,Inherited = false)]
    public class RemarkAttribute:Attribute
    {
        public string _remark { get; set; }

        public RemarkAttribute(string remark)
        {
            _remark = remark;
        }
    }
    public static class RemarkExtension
    {
        
        public static string GetRemark(this Enum @enum)
        {
            string str = null;
            Type type = @enum.GetType();
            string _enum_name= Enum.GetName(type, @enum);
            FieldInfo fieldInfo = type.GetField(@enum.ToString());
            if (fieldInfo != null&& fieldInfo.IsDefined(typeof(RemarkAttribute), true))
            {
                    RemarkAttribute _remarkAttribute = (RemarkAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(RemarkExtension));
                    str = _remarkAttribute._remark;
                    Console.WriteLine($"Get Remark:{str}");
            }
            else { Console.WriteLine($"Cannot Get Remark: {_enum_name}"); }
            return str;
        }
        
    }
}

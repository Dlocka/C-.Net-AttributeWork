using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerDAL
{
    //public static class AttributeCache
    //{
    //    private static Dictionary<(Type,string), Attribute> Dic_ClassAttributes=new Dictionary<(Type, string), Attribute>();
    //    private static Dictionary<(PropertyInfo,string), Attribute> Dic_PropertyAttributes=new Dictionary<(PropertyInfo, string), Attribute>();
    //    public static Attribute GetClassAttribute(Type ClassType,Type AttributeType)
    //    {
    //        if (ClassType.IsDefined(AttributeType))
    //        {
    //            if (!Dic_ClassAttributes.TryGetValue((ClassType, AttributeType.Name), out var attribute))
    //            {
    //                attribute = ClassType.GetCustomAttribute(AttributeType, false);
    //                Dic_ClassAttributes.Add((ClassType, AttributeType.Name), attribute);
    //                return attribute;
    //            }
    //            return Dic_ClassAttributes[(ClassType, AttributeType.Name)];
    //        }
    //        else {
    //            throw new Exception($"{ClassType.Name}类不包含{AttributeType.Name}特性");
    //            return null;}
    //    }

    //    public static Attribute GetPropertyAttribute(PropertyInfo propertyInfo, Type AttributeType)
    //    {
    //        if (propertyInfo.IsDefined(AttributeType,false))
    //        {
    //            if (!Dic_PropertyAttributes.TryGetValue((propertyInfo, AttributeType.Name), out var attribute))
    //            {
    //                attribute = propertyInfo.GetCustomAttribute(AttributeType, false);
    //                Dic_PropertyAttributes.Add((propertyInfo, AttributeType.Name), attribute);
    //                return attribute;
    //            }
    //            return Dic_PropertyAttributes[(propertyInfo, AttributeType.Name)];
    //        }
    //        else { throw new Exception("${propertyInfo.Name}不包含{AttributeType.Name}特性"); }
           
    //    }

    //}
}

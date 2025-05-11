using Models;
using System.Reflection;
namespace Models.AttributeHelper
{
    public static class AttributeHelper
    {
        public static string? GetTableName(Type type)
        {
            if (type.IsDefined(typeof(TableNameAttribute), false))
            {
                var tableNameAttribute = (TableNameAttribute)AttributeCache.GetClassAttribute(type, typeof(TableNameAttribute));
                string _Table_Name = tableNameAttribute.TableName;
                return _Table_Name;
            }
            else
            {
                Console.WriteLine("缺少数据表名Attribute");
                return type.Name;
            }
        }

        public static string GetFieldName(PropertyInfo property)
        {
            string fieldName;
            if (property.IsDefined(typeof(PropertyInDataBaseAttribute), false))
            {
                var propertyNameInDataBaseAttribute = (PropertyInDataBaseAttribute)AttributeCache.GetPropertyAttribute(property, typeof(PropertyInDataBaseAttribute));
                if (propertyNameInDataBaseAttribute.name_in_database == null)
                {
                    fieldName = property.Name;
                }
                else { fieldName = propertyNameInDataBaseAttribute.name_in_database.ToString(); }

            }
            else { fieldName = property.Name; }
            return fieldName;
        }
    }
}

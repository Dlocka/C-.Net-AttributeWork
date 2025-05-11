using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TableNameAttribute:Attribute
    {
        public TableNameAttribute(string tableName)
        {
            TableName = tableName;
        }

        public string TableName { get; set; }
    }
}

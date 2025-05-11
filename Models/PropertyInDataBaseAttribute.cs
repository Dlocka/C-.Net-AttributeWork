using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PropertyInDataBaseAttribute:Attribute
    {
        public PropertyInDataBaseAttribute(string name_in_database)
        {
            this.name_in_database = name_in_database;
        }
        //
        public PropertyInDataBaseAttribute(Type type)
        { 
        }
        public int coloumn_in_database { get; set; }

        public string name_in_database { get; set; }
        public Type type_in_database { get; set; }

    }
}

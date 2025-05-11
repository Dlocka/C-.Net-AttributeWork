using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [TableName("Companies")]
    public class CompanyModel:BaseModel
    {
        [PropertyInDataBase("Name")]
        public string Name {  get; set; }
        public string Remark { get; set; }
        public int Amount { get;set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [TableName("Users")]
    public class UserModel:BaseModel
    {
        [PropertyInDataBase("state")]
        public UserStatus status { get; set; }
    }
   
        public enum UserStatus
        {
        
            Active=1,
            Inactive=2,
            Suspended=3,
            Deleted=4
        }
    }

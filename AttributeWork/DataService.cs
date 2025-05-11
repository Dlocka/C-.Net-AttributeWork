using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using SQLServerDAL;
using Models;
namespace AttributeWork
{
    public static class DataService
    {
        /// <summary>
        /// Search all items of a kind
        /// </summary>
        /// <typeparam name="SearchModel"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetDataTable<SearchModel>() where SearchModel : BaseModel
        {
            var tableNameAttribute = (TableNameAttribute)GetAttribute(typeof(SearchModel), typeof(TableNameAttribute));
            //获取string表名
            string TableName = tableNameAttribute.TableName;
            //创建相应model，数据表
           
            return new DataTable(TableName);
        }
        /// <summary>
        /// 根据Id，获取一条数据
        /// </summary>
        /// <typeparam name="SearchModel">搜索的类型</typeparam>
        /// <param name="_Id"></param>
        /// <returns></returns>
        public static SearchModel GetModel<SearchModel>(int _Id)where SearchModel : BaseModel
            {
            var tableNameAttribute = (TableNameAttribute)GetAttribute(typeof(SearchModel), typeof(TableNameAttribute));
            //获取string表名
            string TableName = tableNameAttribute.TableName;
            string query = $"Select *from {TableName} where id=@id";
            
           
            //创建相应model
            SearchModel model =Activator.CreateInstance(typeof(SearchModel)) as SearchModel;
              return model;
            
            }
        /// <summary>
        /// 更新某一条数据
        /// </summary>
        /// <typeparam name="RefreshModel"></typeparam>
        /// <param name="_refreshModel"></param>
        /// <returns></returns>
        public static bool RefreshSingleData<RefreshModel>(RefreshModel _refreshModel) where RefreshModel : BaseModel
        {
            bool IsSuccess=false;
            //获取表名
            var tableNameAttribute = (TableNameAttribute)GetAttribute(typeof(RefreshModel), typeof(TableNameAttribute));
            //创建相应model
            string TableName = tableNameAttribute.TableName;
            //获取数据
            return IsSuccess;
        }
      

        /// <summary>
        /// 获取Attribute
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="AttributeType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static Attribute GetAttribute(MemberInfo memberInfo, Type AttributeType)
        {
            if (memberInfo == null || AttributeType == null)
            {
                throw new ArgumentNullException("MemberInfo and attributeType parameters cannot be null");
            }
            // Get the attribute applied to the class
            var attribute = Attribute.GetCustomAttribute(memberInfo, AttributeType);
            if (attribute != null)
            {
                Console.WriteLine($"Got Attribute:{AttributeType.Name}");
                return attribute;
            }
            else
            {
                Console.WriteLine($"Not Got Attribute");
                return attribute;
            }
        }


    }
}

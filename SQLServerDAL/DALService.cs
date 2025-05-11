using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Reflection;
using System.ComponentModel;
using IDAL;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ExpressionExtend;
using Models.AttributeHelper;
using static System.Collections.Specialized.BitVector32;
namespace SQLServerDAL
{
    public  class DALService:IServiceDAL
    {
        /// <summary>
        /// 根据Id更改一条数据
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="model"></param>
        public  int UpdateModel <Model>(Model model) where Model : BaseModel
        {
            Type type = model.GetType();
            PropertyInfo IdPro= type.GetProperty("Id");
            
            string TableName = AttributeHelper.GetTableName(type);
            string SqlPropertiesChange=null;
            var properties=type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            //设置SQL语句的参数
            SqlParameter[] sqlParameters=properties.Select(p=>new SqlParameter("@"+AttributeHelper.GetFieldName(p),p.GetValue(model))
            ).ToArray();
            //匹配SQL字段和参数
            SqlPropertiesChange = string.Join(",",properties.Select(p => $"{AttributeHelper.GetFieldName(p)}=@{AttributeHelper.GetFieldName(p)}"));

            //SET FirstName = '@FirstName', LastName = '@LastName', Position = '@Position'
            string SqlQuery = $"UPDATE {TableName} SET {SqlPropertiesChange} WHERE ID ={model.Id.ToString()};";
            int AffectedRows=DBHelper.ExecuteNonQuery(SqlQuery,sqlParameters);
            return AffectedRows;
        }
        public int UpdateBatch<T>(Expression<Func<T,bool>> predicate, Expression<Action<T>> updateExpression) where T : BaseModel,new()
        {
            SqlConditionBuilderVisitor Conditionvisitor = new SqlConditionBuilderVisitor();
            Conditionvisitor.Visit(predicate);
            string SqlCondition = Conditionvisitor.Condition();

           string TableName=AttributeHelper.GetTableName(typeof(T));
            SqlSetBuilderVisitor sqlSetBuilder=new SqlSetBuilderVisitor();
           
           sqlSetBuilder.Visit(updateExpression);
            string SqlSet = sqlSetBuilder.Condition();
            string Sql = String.Format("UPDATE {0} SET {1} Where {2}", TableName, SqlSet, SqlCondition);
           int result= DBHelper.ExecuteNonQuery(Sql);
            return result;
        }
        /// <summary>
        /// Expression表达式 删除条件数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Source"></param>
        /// <param name="expr"></param>
        public static int DeleteBatch<T>(IQueryable<T> Source, Expression<Func<T,bool>>expr) where T : BaseModel
        { 
            SqlConditionBuilderVisitor visitor = new SqlConditionBuilderVisitor();
            visitor.Visit(expr);
            String sql = string.Format("Delete from [{0}]  where{1}", AttributeHelper.GetTableName(typeof(T)), //该模型在数据库的表名
                visitor.Condition());
            int result=DBHelper.ExecuteNonQuery(sql);
            return result;
        }

        public static void Query<T>(IQueryable<T> Source, Expression<Func<T, bool>> expr) where T : BaseModel
        {
            SqlConditionBuilderVisitor visitor = new SqlConditionBuilderVisitor();
            visitor.Visit(expr);
            String sql = string.Format("Select from [{0}]  where{1}", AttributeHelper.GetTableName(typeof(T)), //该模型在数据库的表名
               visitor.Condition());
            DBHelper.ExecuteNonQuery(sql);
        }
    /// <summary>
    /// 通过匹配一个字段来获取数据，并返回第一条
    /// </summary>
    /// <typeparam name="Model"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="_match_value"></param>
    /// <returns></returns>
    public  Model GetDefaultValue<Model>(string propertyName,object _match_value) where Model : BaseModel, new()
        {
            string _fieldName=null;
            string _Table_Name=null;
            Type type = typeof(Model);
            //找到数据库表名
            _Table_Name= AttributeHelper.GetTableName(type);

            #region 找到属性在数据库中对应的字段名
            PropertyInfo property = type.GetProperty(propertyName);
            _fieldName = AttributeHelper.GetFieldName(property);

            #endregion
            string query = $"Select *from {_Table_Name} where {_fieldName}=@ParValue";
            SqlParameter[] paras=new SqlParameter[] { new SqlParameter("@ParValue",_match_value) };
            SqlDataReader reader=DBHelper.ExecuteReader(query, paras);
            if (reader.Read())
                //读取一行数据
            {
                Model _model=new Model();
                var properties = typeof(Model).GetProperties();
                //对每个属性进行赋值
                foreach (var propertyInClass in properties)
                {
                   
                    _fieldName= AttributeHelper.GetFieldName(propertyInClass);
                    //获取字段值
                    //找到在第几行
                    int columnIndex = reader.GetOrdinal(_fieldName);
                    // 根据数据类型读取值
                    object value = reader.GetValue(columnIndex);
                    //属性的赋值
                    propertyInClass.SetValue(_model, value);
                }
               
                return _model;
            }
            else { return null; }
            //string TableName = tableNameAttribute.TableName;
            //return new Models();
        }

   public void Query<T>(Expression<Func<T,bool>> expression) where T : BaseModel
        {

        }

        /// <summary>
        /// 把一组对象的数据插入对应的表格中
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name=""></param>
        public  int Insert<Model>(List<Model> model_list)where Model : BaseModel, new() 
        {
            int AffectedRows=0;
            List<string> PropertyNames=new List<string>();
            var properties = typeof(Model).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            string tableName = ((TableNameAttribute)AttributeCache.GetClassAttribute(typeof(Model), typeof(TableNameAttribute))).TableName;
            string SQLPropertyStr = string.Join(",", properties.Select(p => AttributeHelper.GetFieldName(p)));//SQL语句中的属性
            string SQLValueStr = null;
            //拼接所有对象的值的Sql语句
            //foreach (var item in model_list) 
            //{
            //     string  CurrentValues= string.Join(
            //        ",", properties.Select(p => p.GetValue(item).ToString())
            //        ); //单个对象属性在SQL语句中的值
            //    CurrentValues = "("+CurrentValues+")";
            //    SQLValueStr = SQLValueStr + CurrentValues;
            //}
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<string> ParameterStr=new List<string>(); //用于SQL语句的参数字符串
            for (int i = 0; i < model_list.Count; i++)
            {
                //参数SQL语句拼接
                string CurrentValues = string.Join(",", properties.Select(p => $"@{p.Name}" + "_" + i.ToString()));
                CurrentValues = "(" + CurrentValues + ")";
                ParameterStr.Add(CurrentValues);
                //参数设置
                SqlParameter[] currentParameters = properties.Select(p => new SqlParameter($"@{p.Name}" + "_" + i.ToString(),
                    p.GetValue(model_list[i])??DBNull.Value)).ToArray();
                parameters.AddRange(currentParameters);
            }
            SQLValueStr=string.Join(",",ParameterStr);
            string query = $"Insert into " + $"{tableName}" + "(" + SQLPropertyStr + ")" + "VALUES" +  SQLValueStr ;
            AffectedRows += DBHelper.ExecuteNonQuery(query,parameters.ToArray());
            ////拼接字段的字符串
            #region
            //for (int i = 0; i < properties.Length; i++)
            //{               
            //    string fieldName = GetFieldName(properties[i]);
            //    if(i== 0)
            //    {

            //        SQLPropertyStr +=fieldName;
            //    }
            //    else 
            //    {
            //        SQLPropertyStr += ","+fieldName; 
            //    }
            //}
            #endregion
            #region
            //model插入数据表
            //foreach (var model in model_list)
            //{
            //    //拼接values的字符串
            //    for (int i = 0; i < properties.Length; i++)
            //    {
            //        string FieldValueStr = properties[i].GetValue(model).ToString();
            //        if (GetPropertyType(properties[i])==typeof(string))
            //        {
            //            FieldValueStr = "\'" + FieldValueStr + "\'";
            //        }
            //         if (i == 0) 
            //        {
            //            SQLValueStr += FieldValueStr;
            //            //判断数据表中的类型
            //        }
            //        else
            //        {
            //            SQLValueStr += ","+ FieldValueStr;
            //        }

            //    }

            //    string query = $"Insert into " + $"{tableName}" + "(" + SQLPropertyStr + ")" + "VALUES" + "(" + SQLValueStr + ")";
            //    AffectedRows+=DBHelper.ExecuteNonQuery(query);
            //    SQLValueStr = null;
            //}
            #endregion
            return AffectedRows;
        }
        /// <summary>
        /// 用SqlDataReader读取数据表一行的数据，把数据存入Model
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="reader"></param>
        /// <param name="model"></param>
        public static void LoadFromReader<Model>(SqlDataReader reader, Model model, string _fieldname, PropertyInfo property) where Model : BaseModel
        {
            //获取表名
            //_fieldName = GetFieldName(property);
            ////获取字段值
            ////找到在第几行
            //int columnIndex = reader.GetOrdinal(_fieldname);
            //// 根据数据类型读取值
            //object value = reader.GetValue(columnIndex);
            ////属性的赋值
            //property.SetValue(model, value);

            //UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
            //UserName = reader.GetString(reader.GetOrdinal("UserName"));
            //Email = reader.GetString(reader.GetOrdinal("Email"));
        }
        #region
        /// <summary>
        /// 获取字段名
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        //private static string GetFieldName(PropertyInfo property)
        //{
        //    string fieldName;
        //    if (property.IsDefined(typeof(PropertyInDataBaseAttribute), false))
        //    {
        //        var propertyNameInDataBaseAttribute = (PropertyInDataBaseAttribute)AttributeCache.GetPropertyAttribute(property, typeof(PropertyInDataBaseAttribute));
        //        if (propertyNameInDataBaseAttribute.name_in_database==null)
        //        {
        //            fieldName = property.Name;
        //        }
        //        else { fieldName = propertyNameInDataBaseAttribute.name_in_database.ToString(); }

        //    }
        //    else { fieldName = property.Name; }
        //    return fieldName;
        //}
        //private static int GetFieldColumn(PropertyInfo property)
        //{
        //    int column;
        //    if (property.IsDefined(typeof(PropertyInDataBaseAttribute), false))
        //    {
        //        var ropertyNameInDataBaseAttribute = (PropertyInDataBaseAttribute)property.GetCustomAttribute(typeof(PropertyInDataBaseAttribute), false);
        //        column = ropertyNameInDataBaseAttribute.name_in_database.ToString();
        //    }
        //    else { fieldName = property.Name; }
        //    return column;

        //}
        #endregion

        #region
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// 
        //private static string? GetTableName(Type type)
        //{
        //    if (type.IsDefined(typeof(TableNameAttribute), false))
        //    {
        //        var tableNameAttribute = (TableNameAttribute)AttributeCache.GetClassAttribute(type,typeof(TableNameAttribute));
        //       string  _Table_Name = tableNameAttribute.TableName;
        //        return _Table_Name;
        //    }
        //    else
        //    {
        //        Console.WriteLine("缺少数据表名Attribute");
        //        return type.Name;
        //    }
        //}
        #endregion

        private static Type GetPropertyType(PropertyInfo property)
        {
            if(property.IsDefined(typeof(PropertyInDataBaseAttribute),false))
            {
                PropertyInDataBaseAttribute propertyInDataBase = (PropertyInDataBaseAttribute)property.GetCustomAttribute<PropertyInDataBaseAttribute>();
                if(propertyInDataBase.type_in_database!=null)
                {
                    return propertyInDataBase.type_in_database;
                }
                else 
                {
                    return property.PropertyType;
                }
            }
            else { return property.PropertyType; }
        }

    }
}

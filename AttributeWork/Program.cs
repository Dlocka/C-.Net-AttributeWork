using IDAL;
using Models;
using SQLServerDAL;
using ExpressionExtend;
using System.Linq.Expressions;
using System.Reflection;
namespace AttributeWork
{
    internal class Program
    {
        public static void update(CompanyModel company, PropertyInfo changeProperty, object value)
        {

        }
        static void Main(string[] args)
       {
            IServiceDAL serviceDAL = ServiceFactory.CreateUserService();

            //var properties = typeof(UserModel).GetProperties();
            //foreach (var property in properties)
            //{
            //    Console.WriteLine(property.Name);
            //}
            Console.WriteLine("Hello, World!");
            #region 测试查询
            //UserModel model = serviceDAL.GetDefaultValue<UserModel>("Id", 10001);
            //Console.WriteLine(model.Id);
            //Console.WriteLine(model.status.ToString());
            #endregion

            #region 插入数据
            //List<CompanyModel> companyModels = new List<CompanyModel>() {
            //    new CompanyModel() { Name="澜起科技",Remark="科技公司"} ,
            //    new CompanyModel() { Name="腾讯控股",Remark="科技公司"} };
            //int affectedRows = serviceDAL.Insert(companyModels);
            //Console.WriteLine("AffectedRows:" + affectedRows.ToString());
            #endregion
            //#region 更新数据
            //CompanyModel companymodel = new CompanyModel() { Id=10012,Name = "值得买" ,Remark="笑笑笑"};
            //int AffectedRows = serviceDAL.UpdateModel(companymodel);
            //Console.WriteLine(AffectedRows.ToString());
            //Console.ReadKey();
            //#endregion

            #region
            //Expression<Func<CompanyModel, bool>> lamda = x => x.Name == "腾讯控股" || x.Name == "澜起科技";
            //List<CompanyModel> companies = new List<CompanyModel>();
            //int DeleteResult=DALService.DeleteBatch(companies.AsQueryable(), lamda);
            //Console.WriteLine("Deleted rows:"+DeleteResult.ToString());
            #endregion

            #region

            
            Expression<Func<CompanyModel, bool>> Condition = x => x.Remark == "科技公司";
            CompanyModel company = new CompanyModel();
            PropertyInfo changeProperty = typeof(CompanyModel).GetProperty("Name");
            company.Name = company.Name + 500;

            var parameter = Expression.Parameter(typeof(CompanyModel), "company");
            // 2. 创建属性的 MemberExpression
            var property = Expression.Property(parameter, "Amount");
            var ConstantExpression = Expression.Constant(123);
            var addExpression= Expression.Add(property, ConstantExpression);
            var assignExpression = Expression.Assign(property, addExpression);

            var lambda = Expression.Lambda<Action<CompanyModel>>(assignExpression, parameter);

            //Expression<Action<CompanyModel>> ActionExpression = (company)=> update(company, changeProperty, company.Name+"1234");
           int result = serviceDAL.UpdateBatch<CompanyModel>(Condition, lambda);
           //Console.WriteLine("UpdateRows:" + update.ToString());
            #endregion

        }
        
     }

}

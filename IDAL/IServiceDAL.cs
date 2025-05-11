using Models;
using System.Linq.Expressions;
namespace IDAL
{
    public interface IServiceDAL
    {
        public int UpdateModel<Model>(Model model) where Model : BaseModel;
        public Model GetDefaultValue<Model>(string propertyName, object _match_value) where Model : BaseModel, new();
        public  int Insert<Model>(List<Model> model_list) where Model : BaseModel, new();
        public int UpdateBatch<T>(Expression<Func<T, bool>> predicate, Expression<Action<T>>updateExpression) where T : BaseModel,new();
    }
}

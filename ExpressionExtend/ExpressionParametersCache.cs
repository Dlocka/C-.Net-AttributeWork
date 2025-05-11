using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace ExpressionExtend
{
  
    public static class ExpressionParametersCache
    {
        public static Dictionary<Expression,List<ParameterExpression>> Dic_ExpressionParameters=new Dictionary<Expression,List<ParameterExpression>>();
        public static List<ParameterExpression> GetParameters(Expression expression)
        {
            if (Dic_ExpressionParameters.TryGetValue(expression, out var parameters))
            {
                return parameters;
            }
            else
            {
                List<ParameterExpression> list = new List<ParameterExpression>();
                Dic_ExpressionParameters.Add(expression, list);
                return list;
            }
        }
    }
}

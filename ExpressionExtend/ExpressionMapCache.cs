using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace ExpressionExtend
{
    internal static class ExpressionMapCache
    {
        /// <summary>
        /// 参数匹配字典,<target, srouce>
        /// </summary>
        public static Dictionary<ParameterExpression, ParameterExpression> Dic_ParameterToParameter=new Dictionary<ParameterExpression, ParameterExpression>();
        /// <summary>
        /// get having mapped parameters
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static ParameterExpression GetMappedParameter(this ParameterExpression parameter)
        {
            if (Dic_ParameterToParameter.TryGetValue(parameter, out var mappedParameter))
            {
                return mappedParameter;
            }
            else 
            {
                throw new InvalidOperationException();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionExtend
{
    /// <summary>
    /// 把表达式的所有参数依次列入List
    /// </summary>
    public class GetParameters_ExpressionVisitor:ExpressionVisitor
    {
        //参数列表list
        public static List<ParameterExpression> parameters = new List<ParameterExpression>();
        /// <summary>
        /// 获取对应的参数列表list
        /// </summary>
        /// <param name="_parameters">Expression参数加入的参数list</param>
        public GetParameters_ExpressionVisitor(List<ParameterExpression> _parameters)
        {
            parameters = _parameters;
        }

        public   Expression RecordParameters(Expression expression)
        {
            parameters.Clear();
            return this.Visit(expression);
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            parameters.Add(node);
            return base.VisitParameter(node);
        }
    }
}

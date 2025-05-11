using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ExpressionExtend
{
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _parameterMap;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> parameterMap)
        {
            _parameterMap = parameterMap ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        //public  Expression ReplaceParameters(Expression source, Expression target)
        //{
         
        //}

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> parameterMap, Expression expression)
        {
            return new ParameterRebinder(parameterMap).Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_parameterMap.TryGetValue(node, out var replacement))
            {
                node = replacement;
            }
            return base.VisitParameter(node);
        }
    }
}

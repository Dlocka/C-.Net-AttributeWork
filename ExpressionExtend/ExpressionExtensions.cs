using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace ExpressionExtend
{
    internal static class ExpressionExtensions
    {
        /// <summary>
        /// 匹配两个Expression的所有参数，加入字典
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void MappingParameters(this Expression target, Expression source)
        {
            if (source == null) { return; }
            if (target == null) { return; }

            //1. 从缓存获取Expression参数列表，如果没有则返回一个新建的list，并存入缓存
            List<ParameterExpression> source_parameters = ExpressionParametersCache.GetParameters(source);
            List<ParameterExpression> target_parameters = ExpressionParametersCache.GetParameters(target);
            
            //2. 该Visitor将Expression的参数加入list中
            GetParameters_ExpressionVisitor source_visitor = new GetParameters_ExpressionVisitor(source_parameters);
            source_visitor.RecordParameters(source);
            GetParameters_ExpressionVisitor target_visitor = new GetParameters_ExpressionVisitor(target_parameters);
            target_visitor.RecordParameters(target);

            if(source_parameters.Count!=target_parameters.Count)
            {
                return; 
            }

            //判断类型，如果不一致，说明有误，直接返回
            for (int i = 0; i < source_parameters.Count; i++)
            {
                if (source_parameters[i].Type != target_parameters[i].Type)
                { return; }
            }
            for (int i = 0; i < source_parameters.Count; i++)
            {
                ExpressionMapCache.Dic_ParameterToParameter.Add(target_parameters[i],source_parameters[i]);
            }
        }
        public static void ParameterReplace(this Expression targetExpression, Expression sourceExpression)
        {
            MappingParameters(targetExpression, sourceExpression);
            if (ExpressionParametersCache.Dic_ExpressionParameters.TryGetValue(targetExpression, out var parameters))
            {
                for (var i = 0; i < parameters.Count; i++)
                {
                    parameters[i] = parameters[i].GetMappedParameter();
                }
            }
            else
            {
                Console.WriteLine("目标Expression并未加入字典");
                throw new InvalidOperationException();
            }
        }
    }
}

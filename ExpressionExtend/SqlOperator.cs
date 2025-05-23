﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExpressionExtend
{
    internal static class SqlOperator
    {
        internal static string ToSqlOperator(this ExpressionType type)
        {
            switch (type)
            {
                case (ExpressionType.AndAlso):
                case (ExpressionType.And):
                    return "AND";
                case (ExpressionType.OrElse):
                case (ExpressionType.Or):
                    return "OR";
                case (ExpressionType.Not):
                    return "NOT";
                case (ExpressionType.NotEqual):
                    return "<>";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case (ExpressionType.Equal):
                    return "=";
                case ExpressionType.Assign:
                    return  "=";

                case ExpressionType.Multiply:
                    return "*";
                case ExpressionType.Divide:
                    return "/";
                    
                case ExpressionType.Modulo:
                    return "%";
                   
                case ExpressionType.Subtract:
                    return "-";
                case ExpressionType.Add:
                    return "+";
                default:
                    throw new Exception("不支持该方法");
            }

        }
    }
}

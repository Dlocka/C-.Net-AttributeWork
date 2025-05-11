using Models.AttributeHelper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExpressionExtend
{
    public class SqlSetBuilderVisitor:ExpressionVisitor
    {
        private StringBuilder _sqlBuilder=new StringBuilder();
        private Stack<string> _StringStack = new Stack<string>();
        int _BlockCount = 0;
        int _VistedBlock = 0;
        public SqlSetBuilderVisitor visitor { get; set; }

        public string Condition()
        {
            string condition = string.Concat(this._StringStack.ToArray());
            this._StringStack.Clear();
            return condition;
        }
        private void ChangeBinaryNodeToMark(BinaryExpression node,string Mark)
        {
            base.Visit(node.Right);
            _StringStack.Push(Mark);
            base.Visit(node.Left);
        }
        /// <summary>
        /// 二元表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node == null) throw new ArgumentNullException("BinaryExpression");
            this._StringStack.Push(")");
            base.Visit(node.Right);//解析右边
            this._StringStack.Push(" " + node.NodeType.ToSqlOperator() + " ");
            base.Visit(node.Left);//解析左边
            _StringStack.Push("(");
            return node;
        }
       
        protected override Expression VisitBlock(BlockExpression node)
        {
            List<Expression> Expressions = node.Expressions.Select(Visit).ToList();
            for (int i = 0; i < Expressions.Count-1; i++)
            {
                visitor.Visit(Expressions[i]);
                _StringStack.Push(",");
            }
            visitor.Visit(Expressions.Last());
            return Expression.Block(Expressions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node == null) throw new ArgumentNullException("MemberExpression");
            if (node.Member is PropertyInfo propertyInfo)
            {
                this._StringStack.Push(" [" + AttributeHelper.GetFieldName(propertyInfo) + "] ");

            }
            else
            {
                this._StringStack.Push(" [" + node.Member.Name + "] ");
            }
            return node;
        }

        /// <summary>
        /// 常量表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node == null) throw new ArgumentNullException("ConstantExpression");
            if(node.Type==typeof(string))
            {
                this._StringStack.Push("'"+node.Value.ToString()+"'");
            }
            
            this._StringStack.Push(node.Value.ToString());
            return node;
        }


        /// <summary>
        /// 方法表达式
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m == null) throw new ArgumentNullException("MethodCallExpression");

            string format;
            switch (m.Method.Name)
            {
                case "StartsWith":
                    format = "({0} LIKE {1}+'%')";
                    break;

                case "Contains":
                    format = "({0} LIKE '%'+{1}+'%')";
                    break;

                case "EndsWith":
                    format = "({0} LIKE '%'+{1})";
                    break;

                default:
                    throw new NotSupportedException(m.NodeType + " is not supported!");
            }
            this.Visit(m.Object);
            this.Visit(m.Arguments[0]);
            string right = this._StringStack.Pop();
            string left = this._StringStack.Pop();
            this._StringStack.Push(String.Format(format, left, right));

            return m;
        }
        
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (node.Body is BinaryExpression binaryExpression) 
            { 
                this.VisitFirstBinary(binaryExpression);
                return node;
            }
            this.Visit(node.Body);
            //if (node.Body is MethodCallExpression methodCall)
            //{
            //    VisitMethodCall(methodCall);
            //}
            // Remove the last comma and space
            return node;
        }

        private Expression VisitFirstBinary(BinaryExpression node)
        {
            if (node == null) throw new ArgumentNullException("BinaryExpression");
            base.Visit(node.Right);//解析右边
            this._StringStack.Push(" " + node.NodeType.ToSqlOperator() + " ");
            base.Visit(node.Left);//解析左边
            return node;
        }
    }
}

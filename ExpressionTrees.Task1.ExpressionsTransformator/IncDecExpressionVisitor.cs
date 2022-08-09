using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        private static readonly Expression One = Expression.Constant(1);
        private Dictionary<string, int> _variableNames = new Dictionary<string, int>();

        public void SetTransformationParameters(Dictionary<string, int> parameters)
        {
            _variableNames = parameters;
        }

        public override Expression Visit(Expression node)
        {
            if (node.NodeType == ExpressionType.Add)
            {
                var operation = node as BinaryExpression;
                return Expression.Add(operation.Left, One);
            }

            if (node.NodeType == ExpressionType.Subtract)
            {
                var operation = node as BinaryExpression;
                return Expression.Subtract(operation.Left, One);
            }
            return base.Visit(node);
        }


        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (_variableNames.TryGetValue(node.Parameters.First().Name, out int value))
            {
                var transformedValue = Expression.Constant(value);
                switch (node.Body.NodeType)
                {
                    case ExpressionType.Add:
                        {
                            var operation = Expression.Add(transformedValue, One);
                            return Expression.Lambda<Func<int, int>>(operation, node.Parameters);
                        }
                    case ExpressionType.Subtract:
                        {
                            var operation = Expression.Subtract(transformedValue, One);
                            return Expression.Lambda<Func<int, int>>(operation, node.Parameters);
                        }
                    default:
                        return base.VisitLambda(node);
                }
            }
            return base.VisitLambda(node);
        }
    }
}

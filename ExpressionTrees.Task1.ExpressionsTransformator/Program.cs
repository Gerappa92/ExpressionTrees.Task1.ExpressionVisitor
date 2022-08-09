/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            Expression<Func<int, int>> increment = (variable) => variable + 1;
            Expression<Func<int, int>> decrement = (variable) => variable - 1;

            var visitor = new IncDecExpressionVisitor();
            var incrementExpression = visitor.Visit((increment)) as Expression<Func<int, int>>;
            var incrementFunc = incrementExpression.Compile();
            Console.WriteLine(incrementFunc(7)); //8

            var decrementExpression = visitor.Visit((decrement)) as Expression<Func<int, int>>;
            var decrementFunc = decrementExpression.Compile();
            Console.WriteLine(decrementFunc(7)); //6

            var transformationParameters = new Dictionary<string, int>() { { "variable", 1000 } };

            visitor.SetTransformationParameters(transformationParameters);
            var incrementTransformedExpression = visitor.Visit((increment)) as Expression<Func<int, int>>;
            var incrementTransformedExpressionFunc = incrementTransformedExpression.Compile();
            Console.WriteLine(incrementTransformedExpressionFunc(7)); //1001

            var decrementTransformedExpression = visitor.Visit((decrement)) as Expression<Func<int, int>>;
            var decrementTransformedExpressionFunc = decrementTransformedExpression.Compile();
            Console.WriteLine(decrementTransformedExpressionFunc(7)); //999

            Console.ReadLine();
        }
    }

}

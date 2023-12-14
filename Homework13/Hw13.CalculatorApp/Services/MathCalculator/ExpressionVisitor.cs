using System.Linq.Expressions;
using Hw13.CalculatorApp.ErrorMessages;

namespace Hw13.CalculatorApp.Services.MathCalculator;

public class MathExpressionVisitor
{
    public Expression Visit(Expression node)
        => Visit((dynamic)node);
    
    private Expression Visit(BinaryExpression node)
    {
        Task.WhenAll(
            Task.Run(() => Visit(node.Left)), 
            Task.Run(() => Visit(node.Right)));
        
        return node.NodeType switch
        {
            ExpressionType.Divide when Expression.Lambda<Func<double>>(node.Right).Compile().Invoke() is 0
                => throw new DivideByZeroException(MathErrorMessager.DivisionByZero),
            _ => node
        };
    }

    // унарные операции проверяются предварительно
    private Expression Visit(UnaryExpression node) => node;
    
    private Expression Visit(ConstantExpression node) => node;
}
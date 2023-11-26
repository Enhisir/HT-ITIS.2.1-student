using System.Linq.Expressions;
using Hw11.ErrorMessages;
using Hw11.Exceptions;

namespace Hw11.Services.MathCalculator;

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
            ExpressionType.Add => node,
            ExpressionType.Subtract => node,
            ExpressionType.Multiply => node,
            ExpressionType.Divide when Expression.Lambda<Func<double>>(node.Right).Compile().Invoke() is 0
                => throw new DivideByZeroException(MathErrorMessager.DivisionByZero),
            ExpressionType.Divide => node
        };
    }

    private Expression Visit(UnaryExpression node) 
        => node.NodeType is ExpressionType.Negate 
            ? node 
            : throw new InvalidSyntaxException(MathErrorMessager.TwoOperationInRow);
    
    private Expression Visit(ConstantExpression node) => node;
}
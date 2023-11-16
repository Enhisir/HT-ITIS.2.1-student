using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Hw9.ErrorMessages;

public class ExppressionCalculator : ExpressionVisitor
{
    public override Expression? Visit(Expression? node)
    {
        Task.Delay(1000).Wait();
        return base.Visit(node);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        var expressionValues = CompileBinaryAsync(node).Result;

        var left = expressionValues[0];
        var right = expressionValues[1];
        if (node.NodeType.Equals(ExpressionType.Divide) && right == 0.0)
            throw new DivideByZeroException(MathErrorMessager.DivisionByZero);
        
        return CalculateBinaryExpression(node.NodeType, left, right).Result;
    }

    private async Task<Expression> CalculateBinaryExpression(ExpressionType type, double left, double right)
    {
        Task.Delay(1000).Wait();
        var leftConstant = Expression.Constant(left);
        var rightConstant = Expression.Constant(right);
        
        return type switch
        {
            ExpressionType.Add => Expression.Add(leftConstant, rightConstant),
            ExpressionType.Subtract => Expression.Subtract(leftConstant, rightConstant),
            ExpressionType.Multiply => Expression.Multiply(leftConstant, rightConstant),
            ExpressionType.Divide => Expression.Divide(leftConstant, rightConstant),
        };
    }
    
    private async Task<double[]> CompileBinaryAsync(BinaryExpression node)
    {
        var left = Task.Run(async () =>
        {
            Task.Delay(1000).Wait();
            return Expression.Lambda<Func<double>>(node.Left).Compile().Invoke();
        });
        var right = Task.Run(async () =>
        {
            Task.Delay(1000).Wait();
            return Expression.Lambda<Func<double>>(node.Right).Compile().Invoke();
        });
        var lazyTasks = new[] 
        {
            new Lazy<Task<double>>(left),
            new Lazy<Task<double>>(right),
        };
        return await Task.WhenAll(lazyTasks.Select(l => l.Value));
    }
}
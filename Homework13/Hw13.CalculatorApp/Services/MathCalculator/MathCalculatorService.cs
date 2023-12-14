using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Hw13.CalculatorApp.ErrorMessages;

namespace Hw13.CalculatorApp.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    private readonly ExpressionBuilder _builder = new();
    private readonly MathExpressionVisitor _visitor = new();
    
    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        MathSyntaxValidator.Validate(expression);
        
        var mainExpression = _builder.Parse(expression!);
        var result = _visitor.Visit(mainExpression);
        return await CalculateAsync(result);
    }

    [ExcludeFromCodeCoverage]
    private async Task<double> CalculateAsync(Expression current)
        => await CalculateAsync((dynamic)current);
    
    private async Task<double> CalculateAsync(BinaryExpression current)
    {
        var leftTask = Task.Run(async () =>
        {
            await Task.Delay(1000);
            return await CalculateAsync(current.Left);
        });
        var rightTask = Task.Run(async () =>
        {
            await Task.Delay(1000);
            return await CalculateAsync(current.Right);
        });
        
        return ConvertOperation(
            current.NodeType, 
            await Task.WhenAll(leftTask, rightTask));
    }
    
    private async Task<double> CalculateAsync(UnaryExpression current)
        => -await CalculateAsync(current.Operand);
    
    
    // оставил сингатуру такой же для совместимости с dynamic
    private async Task<double> CalculateAsync(ConstantExpression current)
        => Expression.Lambda<Func<double>>(current).Compile().Invoke();
    
    [ExcludeFromCodeCoverage]
    private static double ConvertOperation(ExpressionType type, IReadOnlyList<double> operands)
    {
        return type switch
        {
            ExpressionType.Add => operands[0] + operands[1],
            ExpressionType.Subtract => operands[0] - operands[1],
            ExpressionType.Multiply => operands[0] * operands[1],
            ExpressionType.Divide when Math.Abs(operands[1]) < double.Epsilon 
                => throw new DivideByZeroException(MathErrorMessager.DivisionByZero),
            ExpressionType.Divide => operands[0] / operands[1],
            ExpressionType.Negate => -operands[0],
            _ => throw new InvalidOperationException("That expression type isn't supported")
        };
    }
}
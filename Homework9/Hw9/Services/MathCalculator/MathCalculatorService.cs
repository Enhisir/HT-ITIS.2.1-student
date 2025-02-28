using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using Hw9.Dto;
using Hw9.ErrorMessages;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        try
        {
            MathSyntaxValidator.Validate(expression);
            
            var treeBuilder = new ExpressionBuilder();
            var visitor = new MathExpressionVisitor();
            
            var mainExpression = treeBuilder.Parse(expression!);
            var executeBefore = visitor.GetExecuteBefore(mainExpression);
            
            var result = await CalculateAsync(mainExpression, executeBefore);
            return new CalculationMathExpressionResultDto(result);
        }
        catch (Exception e)
        {
            return new CalculationMathExpressionResultDto(e.Message);
        }
    }
    
    private async Task<double> CalculateAsync(
        Expression current, 
        IReadOnlyDictionary<Expression, (Expression Left, Expression? Right)> executeBefore)
    {
        if (!executeBefore.ContainsKey(current))
        {
            return double.Parse(current.ToString(), CultureInfo.InvariantCulture);
        }
        var leftTask = Task.Run(async () =>
        {
            await Task.Delay(1000);
            return await CalculateAsync(executeBefore[current].Item1, executeBefore);
        });
        var rightTask = Task.Run(async () =>
        {
            await Task.Delay(1000);
            return executeBefore[current].Right is null 
                ? double.NaN
                : await CalculateAsync(executeBefore[current].Item2!, executeBefore);
        });

        var results = await Task.WhenAll(leftTask, rightTask);
        return Calculate(current.NodeType, results);
    }
    
    [ExcludeFromCodeCoverage]
    private static double Calculate(ExpressionType type, IReadOnlyList<double> operands)
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
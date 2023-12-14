namespace Hw13.CalculatorApp.Services.MathCalculator;

public interface IMathCalculatorService
{ 
    Task<double> CalculateMathExpressionAsync(string? expression);
}
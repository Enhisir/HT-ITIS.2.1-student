namespace Hw8.Calculator;

public class Calculator : ICalculator
{
    public double Plus(double val1, double val2) => val1 + val2;
    
    public double Minus(double val1, double val2) => val1 - val2;

    public double Multiply(double val1, double val2) => val1 * val2;

    public double Divide(double val1, double val2) => val2 == 0 
        ? throw new DivideByZeroException()
        : val1 / val2;
}
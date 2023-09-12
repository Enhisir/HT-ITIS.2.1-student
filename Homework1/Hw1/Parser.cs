namespace Hw1;

public static class Parser
{
    public static void ParseCalcArguments(string[] args, 
        out double val1, 
        out CalculatorOperation operation, 
        out double val2)
    {
        if (!IsArgLengthSupported(args)) 
            throw new ArgumentException(
                "Недостаточно аргументов, их должно быть не менее трех.",
                nameof(args));

        val1 = ParseOperand(args[0]);
        val2 = ParseOperand(args[2]);
        
        operation = ParseOperation(args[1]);
        if (operation == CalculatorOperation.Undefined) 
            throw new InvalidOperationException(nameof(operation));
    }

    private static bool IsArgLengthSupported(string[] args) => args.Length == 3;

    private static CalculatorOperation ParseOperation(string arg)
    {
        return arg switch
        {
            "+" => CalculatorOperation.Plus,
            "-" => CalculatorOperation.Minus,
            "*" => CalculatorOperation.Multiply,
            "/" => CalculatorOperation.Divide,
            _ => CalculatorOperation.Undefined
        };
    }

    private static double ParseOperand(string argValue)
    {
        if (!double.TryParse(argValue, out var result)) 
            throw new ArgumentException("операнд не является числом.");

        return result;
    }
}
namespace Hw1;

public static class Parser
{
    public static void ParseCalcArguments(string[] args, 
        out double val1, 
        out CalculatorOperation operation, 
        out double val2)
    {
        if (args is null) 
            throw new ArgumentNullException(nameof(args));

        if (!IsArgLengthSupported(args)) 
            throw new ArgumentException(
                "Недостаточно аргументов, их должно быть не менее трех.",
                nameof(args));
        
        if (!double.TryParse(args[0], out val1)) 
            throw new ArgumentException(
                "Первый операнд не является числом.", 
                nameof(val1));

        operation = ParseOperation(args[1]);
        if (operation == CalculatorOperation.Undefined) 
            throw new InvalidOperationException(nameof(operation));
        
        if (!double.TryParse(args[2], out val2)) 
            throw new ArgumentException(
                "Второй операнд не является числом.", 
                nameof(val2));
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
}
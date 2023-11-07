using System.Globalization;

namespace Hw8.Calculator;

public static class Parser
{
    public static bool TryParseDouble(string source, out double result) 
        => double.TryParse(source, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
    
    public static Operation ParseOperation(string source)
        => Enum.TryParse(source, out Operation operation) ? operation : Operation.Invalid;
}
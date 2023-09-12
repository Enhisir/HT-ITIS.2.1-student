using Hw1;


try
{
    Parser.ParseCalcArguments(
        args, 
        out var first, 
        out var operation, 
        out var second);
    
    var result = Calculator.Calculate(first, operation, second);
    Console.WriteLine(result);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

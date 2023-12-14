using System.Text.RegularExpressions;
using Hw13.CalculatorApp.ErrorMessages;
using Hw13.CalculatorApp.Exceptions;

namespace Hw13.CalculatorApp.Services.MathCalculator;

public static class MathSyntaxValidator
{
    private static readonly char[] AllCharacters = { '+', '-', '*', '/', '(', ')', '.'};
    
    private static readonly Regex Delimiters = new (@"([\+\-\*\/\(\)])");
    private static readonly Regex Number = new (@"^\d+(\.\d+)?$");
    private static readonly Regex Operation = new (@"^([\+\-\*\/])$");


    public static void Validate(string? expression)
    {
        if (string.IsNullOrEmpty(expression))
            throw new InvalidSyntaxException(MathErrorMessager.EmptyString);

        if (expression.Contains(' '))
            expression = ExpressionBuilder.PrepareString(expression);

        if (BracketsAreInvalid(expression))
            throw new InvalidSyntaxException(MathErrorMessager.IncorrectBracketsNumber);
        
        foreach (var value in expression.Where(c => !(char.IsDigit(c) || AllCharacters.Contains(c))))
            throw new InvalidSymbolException(MathErrorMessager.UnknownCharacterMessage(value));

        var elements = Delimiters.Split(expression);

        string prevElement = null!;
        var prevType = ElementType.Nothing;
        foreach (var element in elements.Where(token => !string.IsNullOrEmpty(token)))
        {
            if (IsOperation(element))
            {
                prevType = prevType switch
                {
                    ElementType.Nothing when !element.Equals("-") 
                        => throw new InvalidSyntaxException(MathErrorMessager.StartingWithOperation),
                    ElementType.OpeningBracket when !element.Equals("-") 
                        => throw new InvalidSyntaxException(
                            MathErrorMessager.InvalidOperatorAfterParenthesisMessage(element)),
                    ElementType.Operation 
                        => throw new InvalidSyntaxException(
                            MathErrorMessager.TwoOperationInRowMessage(prevElement, element)),
                    _ => ElementType.Operation
                };
            }
            else if (element.Equals("("))
            {
                prevType = ElementType.OpeningBracket;
            }
            else if (element.Equals(")"))
            {
                if (prevType == ElementType.Operation)
                    throw new InvalidSyntaxException(MathErrorMessager.OperationBeforeParenthesisMessage(prevElement));
                
                prevType = ElementType.ClosingBracket;
            }
            else
            {
                if (!IsNumber(element))
                    throw new InvalidNumberException(MathErrorMessager.NotNumberMessage(element));
                
                prevType = ElementType.Number;
            }
            prevElement = element;
        }

        if (prevType == ElementType.Operation)
            throw new InvalidSyntaxException(MathErrorMessager.EndingWithOperation);
    }

    private static bool BracketsAreInvalid(string expression)
    {
        var balance = 0;

        foreach (var c in expression)
        {
            switch (c)
            {
                case '(':
                    balance++;
                    break;
                case ')':
                    balance--;
                    if (balance < 0) return true;
                    break;
            }
        }
        
        return balance != 0;
    }

    private static bool IsOperation(string element) => Operation.Match(element).Success;
    
    private static bool IsNumber(string element) => Number.Match(element).Success;
}

internal enum ElementType
{
    Number,
    Operation,
    OpeningBracket,
    ClosingBracket,
    Nothing
}
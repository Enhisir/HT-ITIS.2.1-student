namespace Hw13.CalculatorApp.Exceptions;

public class InvalidSyntaxException : Exception
{
	public InvalidSyntaxException(string message)
		: base(message)
	{
	}
}
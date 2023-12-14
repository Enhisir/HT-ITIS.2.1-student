namespace Hw13.CalculatorApp.Exceptions;

public class InvalidSymbolException: Exception
{
	public InvalidSymbolException(string message)
		: base(message)
	{
	}
}
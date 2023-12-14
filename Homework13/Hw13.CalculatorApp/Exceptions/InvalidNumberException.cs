namespace Hw13.CalculatorApp.Exceptions;

public class InvalidNumberException: Exception
{
	public InvalidNumberException(string message)
		: base(message)
	{
	}
}
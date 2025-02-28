﻿namespace Hw13.CalculatorApp.Exceptions;

public interface IExceptionHandler
{
	public void HandleException<T>(T exception) where T : Exception;
}
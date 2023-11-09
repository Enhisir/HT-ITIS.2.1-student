using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate([FromServices] ICalculator calculator,
        string val1,
        string operation,
        string val2)
    {
        if (!Parser.TryParseDouble(val1, out var firstOp) || !Parser.TryParseDouble(val2, out var secondOp))
            return BadRequest(Messages.InvalidNumberMessage);
        
        
        return Parser.ParseOperation(operation) switch {
            Operation.Plus     => Ok(calculator.Plus(firstOp, secondOp).ToString(CultureInfo.InvariantCulture)),
            Operation.Minus    => Ok(calculator.Minus(firstOp, secondOp).ToString(CultureInfo.InvariantCulture)),
            Operation.Multiply => Ok(calculator.Multiply(firstOp, secondOp).ToString(CultureInfo.InvariantCulture)),
            Operation.Divide   => secondOp == 0 
                ? BadRequest(Messages.DivisionByZeroMessage) 
                : Ok(calculator.Divide(firstOp, secondOp).ToString(CultureInfo.InvariantCulture)),
            _ => BadRequest(Messages.InvalidOperationMessage)
        };
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}
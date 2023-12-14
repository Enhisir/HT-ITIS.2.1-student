using System.Diagnostics.CodeAnalysis;
using Hw13.CalculatorApp.Dto;
using Hw13.CalculatorApp.Exceptions;
using Hw13.CalculatorApp.Services.MathCalculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw13.CalculatorApp.Controllers;

public class CalculatorController : Controller
{
    private readonly IMathCalculatorService _mathCalculatorService;
    private readonly IExceptionHandler _exceptionHandler;

    public CalculatorController(IMathCalculatorService mathCalculatorService, IExceptionHandler exceptionHandler)
    {
        _mathCalculatorService = mathCalculatorService;
        _exceptionHandler = exceptionHandler;
    }
        
    [HttpGet]
    [ExcludeFromCodeCoverage]
    public IActionResult Calculator()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult<CalculationMathExpressionResultDto>> CalculateMathExpression(string expression)
    {
        try
        {
            var result = await _mathCalculatorService.CalculateMathExpressionAsync(expression);
            return Json(new CalculationMathExpressionResultDto(result));
        }
        catch (Exception e)
        {
            _exceptionHandler.HandleException(e);
            return Json(new CalculationMathExpressionResultDto(e.Message));
        }
    }
}
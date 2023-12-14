using Hw13.CalculatorApp.Services.MathCalculator;

namespace Hw13.CalculatorApp.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services.AddTransient<IMathCalculatorService, MathCalculatorService>();
    }
}
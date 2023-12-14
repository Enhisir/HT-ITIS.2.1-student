using System.Diagnostics.CodeAnalysis;
using Hw13.CalculatorApp.Configuration;
using Hw13.CalculatorApp.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddMathCalculator();
builder.Services.AddTransient<IExceptionHandler, ExceptionHandler>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Calculator}/{action=Calculator}/{id?}");

app.Run();

namespace Hw13.CalculatorApp
{
    [ExcludeFromCodeCoverage]
    public partial class Program { }
}
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Hw7.FormComponents;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var model = helper.ViewData.Model;
        var modelType = helper.ViewData.ModelMetadata.ModelType;

        var builder = new StringBuilder();
        foreach (var propertyInfo in modelType.GetProperties())
        {
            builder.Append("<div class=\"form-group\">");
            HtmlField field = propertyInfo.PropertyType.IsEnum 
                ? new HtmlSelect(propertyInfo)
                : new HtmlInput(propertyInfo);
            builder.Append(field.GetHtmlContent());

            var validationMessage = ValidateProperty(propertyInfo, model);

            if (validationMessage is not null)
                builder.Append($"<span class=\"text-danger\">{validationMessage}</small>");
            
            builder.Append("</div>");
        }

        return new HtmlString(builder.ToString());
    }

    private static string? ValidateProperty(PropertyInfo propertyInfo, object? model)
    {
        if (model is null) return null;
        return propertyInfo
            .GetCustomAttributes().
            OfType<ValidationAttribute>()
            .FirstOrDefault(x => !x.IsValid(propertyInfo.GetValue(model)))
            ?.ErrorMessage;
    }
} 
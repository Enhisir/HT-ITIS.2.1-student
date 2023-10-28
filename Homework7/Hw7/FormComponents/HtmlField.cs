using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Hw7.FormComponents;

public abstract class HtmlField
{
    public string Name { get; }
    public Type Type { get; }
    public string? Label { get; }
    
    protected HtmlField(PropertyInfo propertyInfo)
    {
        Name = propertyInfo.Name;
        Type = propertyInfo.PropertyType;
        
        var displayAttr = (DisplayAttribute?)propertyInfo
            .GetCustomAttributes()
            .FirstOrDefault(x => x is DisplayAttribute);
        Label = displayAttr?.Name;
    }
    
    public string GetHtmlContent()
    {
        var builder = new StringBuilder();
        
        builder.Append(GetHtmlLabel());
        builder.Append(GetHtmlField());
        return builder.ToString();
    }
    
    private string GetHtmlLabel()
    {
        var label = Label is not null && Label != string.Empty
            ? Label
            : Regex.Replace(
                Name, 
                "([A-Z])", 
                " $1", 
                RegexOptions.Compiled).Trim();
        return $"<label for=\"{Name}\">{label}</label>";
    }

    protected abstract string GetHtmlField();
}
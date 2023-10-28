using System.Reflection;

namespace Hw7.FormComponents;

public sealed class HtmlInput : HtmlField
{
    public HtmlInput(PropertyInfo propertyInfo) 
        : base(propertyInfo) {}
    
    protected override string GetHtmlField()
    {
        var htmlType = Type == typeof(string) ? "text" : "number";
        return $"<input class=\"form-control\" type=\"{htmlType}\"  name=\"{Name}\" id=\"{Name}\"/>";
    }
}
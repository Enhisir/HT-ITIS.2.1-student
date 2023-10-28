using System.Reflection;
using System.Text;

namespace Hw7.FormComponents;

public sealed class HtmlSelect : HtmlField
{
    public HtmlSelect(PropertyInfo propertyInfo) 
        : base(propertyInfo) {}

    protected override string GetHtmlField()
    {
        var builder = new StringBuilder();

        builder.AppendLine($"<select class=\"form-control\" name=\"{Name}\" required>");
        foreach (var value in Enum.GetValues(Type))
            builder.AppendLine($"<option value=\"{value}\">{value}</option>");

        builder.AppendLine("</select>");

        return builder.ToString();
    }
}
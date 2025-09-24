using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace GestaoDeBarbearia.Api.Filters;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
        {
            return;
        }

        var enumDescriptions = new List<string>();
        foreach (var member in context.Type.GetMembers(BindingFlags.Public | BindingFlags.Static))
        {
            var descriptionAttribute = member.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                var value = (int)Enum.Parse(context.Type, member.Name);
                enumDescriptions.Add($"**{value}** - {member.Name} ({descriptionAttribute.Description})");
            }
        }

        if (enumDescriptions.Count > 0)
        {
            schema.Description += $"<br/>Possíveis valores:<br/>{string.Join("<br/>", enumDescriptions)}";
        }
    }


    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string? ToString()
    {
        return base.ToString();
    }
}
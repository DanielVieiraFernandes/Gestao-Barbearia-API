using System.ComponentModel;
using System.Reflection;

namespace GestaoDeBarbearia.Domain.Enums;
public static class EnumsExtensionMethods
{
    public static string GetEnumDescription<T>(this T enumValue) where T : Enum
    {
        var description = enumValue.ToString();
        var fieldInfo = enumValue.GetType().GetField(description);
            
        if (fieldInfo != null)
        {
            var attributes = fieldInfo.GetCustomAttributes<DescriptionAttribute>(true);
            if (attributes != null && attributes.Any())
            {
                description = ((DescriptionAttribute)attributes.First()).Description;
            }
        }

        return description;
    }

}

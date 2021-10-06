using System;
using System.ComponentModel;
using System.Linq;

namespace Peernet.Browser.Models.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription<T>(this T val) where T : Enum
        {
            var type = typeof(T);
            var memInfo = type.GetMember(type.GetEnumName(val));
            var descriptionAttribute = memInfo[0]
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;
            return descriptionAttribute?.Description;
        }
    }
}
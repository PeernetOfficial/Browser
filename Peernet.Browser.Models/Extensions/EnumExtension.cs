using System.ComponentModel;
using System.Linq;

namespace System
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

        public static bool IsDefault<T>(this T val) where T : Enum => (int)(object)val == 0;

        public static T GetDefault<T>(this T val) where T : Enum => (T)Enum.GetValues(typeof(T)).GetValue(0);
    }
}
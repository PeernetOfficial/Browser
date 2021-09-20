using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list) => list?.Any() != true;

        public static void Foreach<T>(this IEnumerable<T> list, Action<T> a)
        {
            if (list.IsNullOrEmpty() || a == null) return;
            foreach (var l in list) a(l);
        }
    }
}
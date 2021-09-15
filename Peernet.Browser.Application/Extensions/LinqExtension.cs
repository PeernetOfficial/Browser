using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list) => list?.Any() != true;
    }
}
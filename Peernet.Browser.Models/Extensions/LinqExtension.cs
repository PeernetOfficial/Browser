using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.Extensions
{
    public static class LinqExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list) => list?.Any() != true;

        public static void Foreach<T>(this IEnumerable<T> list, Action<T> a)
        {
            if (list.IsNullOrEmpty() || a == null) return;
            foreach (var l in list) a(l);
        }

        public static void RemoveRange<T>(this IList<T> collection, IEnumerable<T> filesToRemove)
        {
            foreach (var selectedFile in filesToRemove)
            {
                collection.Remove(selectedFile);
            }
        }
    }
}
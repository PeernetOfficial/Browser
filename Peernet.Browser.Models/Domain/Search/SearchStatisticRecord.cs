using System;

namespace Peernet.Browser.Models.Domain.Search
{
    public class SearchStatisticRecord<T> where T : Enum
    {
        /// <summary>
        /// Key index. The exact meaning depends on where this structure is used.
        /// </summary>
        public T Key { get; set; }

        /// <summary>
        /// Count of files for the given key
        /// </summary>
        public int Count { get; set; }
    }
}
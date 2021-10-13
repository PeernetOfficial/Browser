using System;

namespace Peernet.Browser.Models.Domain.Search
{
    public class SearchStatisticRecordDay
    {
        /// <summary>
        /// The day (which covers the full 24 hours). Always rounded down to midnight.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Count of files.
        /// </summary>
        public int Count { get; set; }
    }
}
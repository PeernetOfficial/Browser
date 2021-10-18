namespace Peernet.Browser.Models.Domain.Search
{
    public class SearchRequest : SearchRequestBase
    {
        /// <summary>
        /// Search term.
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// Timeout in seconds. 0 means default.
        /// This is the entire time the search may take.
        /// Found results are still available after this timeout.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Total number of max results. 0 means default.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Date from, both from/to are required if set.
        /// </summary>
        public string DateFrom { get; set; }

        /// <summary>
        /// Date to, both from/to are required if set.
        /// </summary>
        public string DateTo { get; set; }

        /// <summary>
        /// Optional: Previous search IDs to terminate.
        /// This is if the user makes a new search from the same tab.
        /// Same as first calling /search/terminate.
        /// </summary>
        public string[] Terminate { get; set; }
    }
}
namespace Peernet.Browser.Models.Domain.Search
{
    public class SearchRequest
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
        ///  Sort order:
        ///  0 = No sorting,
        ///  1 = Relevance ASC,
        ///  2 = Relevance DESC (this should be default),
        ///  3 = Date ASC,
        ///  4 = Date DESC
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// Optional: Previous search IDs to terminate.
        /// This is if the user makes a new search from the same tab.
        /// Same as first calling /search/terminate.
        /// </summary>
        public int[] Terminate { get; set; }

        /// <summary>
        /// 0 = No filters used,
        /// 1 = Use file type filter,
        /// 2 = Use file format filter.
        /// </summary>
        public int TypeFilter { get; set; }

        /// <summary>
        /// File type such as binary, text document etc. See core.TypeX.
        /// </summary>
        public int FileType { get; set; }

        /// <summary>
        /// File format such as PDF, Word, Ebook, etc. See core.FormatX.
        /// </summary>
        public int FileFormat { get; set; }
    }
}
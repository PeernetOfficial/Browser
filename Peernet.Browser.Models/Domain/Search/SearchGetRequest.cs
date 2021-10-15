namespace Peernet.Browser.Models.Domain.Search
{
    public class SearchGetRequest : SearchRequestBase
    {
        public SearchGetRequest() : base()
        {
            Limit = 100;
            Stats = 1;
            Reset = 1;
        }

        /// <summary>
        /// UUID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// max records
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// max records
        /// </summary>
        public int Stats { get; set; }

        /// <summary>
        /// to reset the filters or sort orders with any of the below parameters (all required):
        /// </summary>
        public int Reset { get; set; }

        /// <summary>
        /// Date from, both from/to are required if set.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Date to, both from/to are required if set.
        /// </summary>
        public string To { get; set; }
    }
}
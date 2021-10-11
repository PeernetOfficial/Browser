namespace Peernet.Browser.Models.Domain.Search
{
    public class SearchRequestResponse
    {
        /// <summary>
        /// Status of the search
        /// </summary>
        public SearchRequestResponseStatusEnum Status { get; set; }

        /// <summary>
        /// UUID
        /// </summary>
        public string Id { get; set; }
    }
}
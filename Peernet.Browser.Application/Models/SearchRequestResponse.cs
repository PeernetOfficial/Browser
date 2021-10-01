namespace Peernet.Browser.Application.Models
{
    public class SearchRequestResponse
    {
        /// <summary>
        /// Status of the search: 0 = Success (ID valid), 1 = Invalid Term, 2 = Error Max Concurrent Searches
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// UUID
        /// </summary>
        public string Id { get; set; }
    }
}
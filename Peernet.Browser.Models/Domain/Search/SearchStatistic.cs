namespace Peernet.Browser.Models.Domain.Search
{
    public class SearchStatistic : SearchStatisticData
    {
        public SearchStatusEnum Status { get; set; }

        /// <summary>
        /// Whether the search is terminated, meaning that statistics won't change
        /// </summary>
        public bool Terminated { get; set; }
    }
}
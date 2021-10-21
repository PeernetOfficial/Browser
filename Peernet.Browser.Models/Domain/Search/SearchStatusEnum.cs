namespace Peernet.Browser.Models.Domain.Search
{
    public enum SearchStatusEnum
    {
        Success = 0,
        NoMoreResults = 1,
        IdNotFound = 2,

        /// <summary>
        /// No results yet available keep trying
        /// </summary>
        KeepTrying = 3
    }
}
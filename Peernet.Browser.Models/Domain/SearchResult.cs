using MvvmCross.ViewModels;
using System.Collections.Generic;

namespace Peernet.Browser.Models.Domain
{
    public class SearchResult : MvxNotifyPropertyChanged
    {
        /// <summary>
        /// List of files found
        /// </summary>
        public List<ApiBlockRecordFile> Files { get; set; }

        /// <summary>
        /// Status: 0 = Success with results, 1 = No more results available, 2 = Search ID not found, 3 = No results yet available keep trying
        /// </summary>
        public int Status { get; set; }
    }
}
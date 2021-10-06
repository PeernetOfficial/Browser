using MvvmCross.ViewModels;
using System.Collections.Generic;

namespace Peernet.Browser.Models.Domain
{
    public class SearchResult : MvxNotifyPropertyChanged
    {
        public List<ApiBlockRecordFile> Files { get; set; }

        public int Status { get; set; }
    }
}
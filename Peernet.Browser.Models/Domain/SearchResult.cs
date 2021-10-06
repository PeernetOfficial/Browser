using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace Peernet.Browser.Models.Domain
{
    public class SearchResult : MvxNotifyPropertyChanged
    {
        public List<ApiBlockRecordFile> Files { get; set; }

        public int Status { get; set; }
    }
}
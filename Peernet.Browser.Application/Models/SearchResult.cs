using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Enums;
using System;
using System.Collections.Generic;

namespace Peernet.Browser.Application.Models
{
    public class SearchResult : MvxNotifyPropertyChanged
    {
        public List<ApiBlockRecordFile> Files { get; set; }
        public int Status { get; set; }
    }

    public class SearchResult2 : MvxNotifyPropertyChanged
    {
        public SearchResult2(ApiBlockRecordFile source, Action<SearchResult2> download)
        {
            DownloadCommand = new MvxCommand(() => download?.Invoke(this));

            Name = source.Name;
            Date = source.Date.ToString();
            Size = $"{source.Size} MB";
            SharedBy = $"123 Peers";
        }

        public string Name { get; }
        public string Date { get; }
        public string Size { get; }
        public string SharedBy { get; }
        public HealthType Health { get; }
        public IMvxCommand DownloadCommand { get; }
    }
}
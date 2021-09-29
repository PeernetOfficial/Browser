using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Enums;
using System;

namespace Peernet.Browser.Application.Models
{
    public class SearchResultRow : MvxNotifyPropertyChanged
    {
        public SearchResultRow(ApiBlockRecordFile source, Action<SearchResultRow> download)
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
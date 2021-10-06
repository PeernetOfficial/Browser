using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Models.Domain;
using System;

namespace Peernet.Browser.Models.Presentation
{
    public class SearchResultRowModel : MvxNotifyPropertyChanged
    {
        public SearchResultRowModel(ApiBlockRecordFile source, Action<SearchResultRowModel> download)
        {
            DownloadCommand = new MvxCommand(() => download?.Invoke(this));
            EnumerationMember = (HealthType)int.Parse(source.Id);
            Name = source.Name;
            Date = source.Date.ToString();
            Size = $"{source.Size} MB";
            SharedBy = source.SharedByCount;
            FlameIsVisible = source.Size > 15;
        }

        public HealthType EnumerationMember { get; set; }

        public string Name { get; }
        public string Date { get; }
        public string Size { get; }
        public int SharedBy { get; }
        public bool FlameIsVisible { get; }
        public IMvxCommand DownloadCommand { get; }
    }
}
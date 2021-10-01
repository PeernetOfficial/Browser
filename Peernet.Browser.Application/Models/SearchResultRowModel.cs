using MvvmCross.Commands;
using Peernet.Browser.Application.Enums;
using System;

namespace Peernet.Browser.Application.Models
{
    public class SearchResultRowModel
    {
        public SearchResultRowModel(ApiBlockRecordFile source)
        {
            DownloadCommand = new MvxCommand(() => DownloadAction?.Invoke(this));
            EnumerationMember = (HealthType)(DateTime.Now.Ticks % 4);
            Name = source.Name;
            Date = source.Date.ToString();
            Size = $"{source.Size} MB";
            SharedBy = $"123 Peers";
            FlameIsVisible = source.Size > 15;
        }

        public HealthType EnumerationMember { get; }

        public string Name { get; }
        public string Date { get; }
        public string Size { get; }
        public string SharedBy { get; }
        public bool FlameIsVisible { get; }
        public IMvxCommand DownloadCommand { get; }

        public Action<SearchResultRowModel> DownloadAction { get; set; }
    }
}
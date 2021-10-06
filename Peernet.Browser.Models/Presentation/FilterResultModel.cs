using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;

namespace Peernet.Browser.Models.Presentation
{
    public class FilterResultModel : MvxNotifyPropertyChanged
    {
        public string Content { get; set; }

        public SearchFiltersType Type { get; set; }

        public FilterResultModel(Action<FilterResultModel> click)
        {
            ClickCommand = new MvxCommand(() => click?.Invoke(this));
        }

        public IMvxCommand ClickCommand { get; }
    }
}
using System;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Peernet.Browser.Models.Presentation.Home
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
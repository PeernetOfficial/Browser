using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Enums;
using System;

namespace Peernet.Browser.Application.Models
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
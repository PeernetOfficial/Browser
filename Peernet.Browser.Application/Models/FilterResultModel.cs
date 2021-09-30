using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;

namespace Peernet.Browser.Application.Models
{
    public class FilterResultModel : MvxNotifyPropertyChanged
    {
        private string content;

        public string Content

        {
            get => content;
            set => SetProperty(ref content, value);
        }

        public FilterResultModel(Action<FilterResultModel> click)
        {
            ClickCommand = new MvxCommand(() => click?.Invoke(this));
        }

        public IMvxCommand ClickCommand { get; }
    }
}
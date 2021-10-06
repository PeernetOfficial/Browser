using System;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Peernet.Browser.Models.Presentation
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
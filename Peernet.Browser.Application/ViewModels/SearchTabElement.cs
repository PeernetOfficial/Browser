using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchTabElement : MvxNotifyPropertyChanged
    {
        private string title;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string content;

        public string Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }

        public SearchTabElement(Action<SearchTabElement> deleteAction)
        {
            DeleteCommand = new MvxCommand(() => deleteAction(this));
        }

        public IMvxCommand DeleteCommand { get; }
    }
}
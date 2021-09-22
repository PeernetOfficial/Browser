using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchTabElement : MvxNotifyPropertyChanged
    {
        public SearchContentElement Content { get; }

        public string Title { get; }

        public SearchTabElement(string title, Action<SearchTabElement> deleteAction)
        {
            Title = title;
            Content = new SearchContentElement();
            DeleteCommand = new MvxCommand(() => deleteAction(this));
        }

        public IMvxCommand DeleteCommand { get; }
    }
}
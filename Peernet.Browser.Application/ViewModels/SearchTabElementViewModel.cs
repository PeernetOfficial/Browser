using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchTabElementViewModel : MvxNotifyPropertyChanged
    {
        public SearchContentElementViewModel Content { get; }

        public string Title { get; }

        public SearchTabElementViewModel(string title, Action<SearchTabElementViewModel> deleteAction)
        {
            Title = title;
            Content = new SearchContentElementViewModel();
            DeleteCommand = new MvxCommand(() => deleteAction(this));
        }

        public IMvxCommand DeleteCommand { get; }
    }
}
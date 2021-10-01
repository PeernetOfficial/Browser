using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Services;
using System;

namespace Peernet.Browser.Application.Models
{
    public class SearchTabElement : MvxNotifyPropertyChanged
    {
        public string Title { get; }
        public SearchContentElement Content { get; }

        public SearchTabElement(string title, Action<SearchTabElement> deleteAction, ISearchService searchService)
        {
            Title = title;
            Content = new SearchContentElement(searchService.Search);
            DeleteCommand = new MvxCommand(() => deleteAction(this));
        }

        public IMvxCommand DeleteCommand { get; }
    }
}
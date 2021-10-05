using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Services;
using System;

namespace Peernet.Browser.Application.Models
{
    public class SearchTabElement : MvxNotifyPropertyChanged
    {
        public SearchTabElement(string title, Action<SearchTabElement> deleteAction, ISearchService searchService)
        {
            Title = title;
            Content = new SearchContentElement(new FiltersModel(title, searchService.Search));
            DeleteCommand = new MvxCommand(() => deleteAction(this));
        }

        public SearchContentElement Content { get; }
        public IMvxCommand DeleteCommand { get; }
        public string Title { get; }
    }
}
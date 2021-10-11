using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Presentation;
using Peernet.Browser.Models.Presentation.Home;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchTabElementViewModel : MvxNotifyPropertyChanged
    {
        public SearchContentElementViewModel Content { get; }

        public string Title { get; }

        public SearchTabElementViewModel(string title, Action<SearchTabElementViewModel> deleteAction, ISearchService searchService)
        {
            Title = title;
            Content = new SearchContentElementViewModel(new FiltersModel(title, async model => await searchService.Search(model)));
            DeleteCommand = new MvxCommand(() => deleteAction(this));
        }

        public IMvxCommand DeleteCommand { get; }
    }
}
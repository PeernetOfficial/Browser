using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Models.Presentation.Home;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchTabElementViewModel : MvxNotifyPropertyChanged
    {
        public SearchContentElementViewModel Content { get; }

        public string Title { get; }

        public SearchTabElementViewModel(string title, Action<SearchTabElementViewModel> deleteAction, Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction)
        {
            Title = title;
            Content = new SearchContentElementViewModel(new FiltersModel(title, refreshAction));
            DeleteCommand = new MvxCommand(() => deleteAction(this));
        }

        public IMvxCommand DeleteCommand { get; }
    }
}
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

        public SearchTabElementViewModel(string title, Func<SearchTabElementViewModel, Task> deleteAction, Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction, Func<SearchResultRowModel, Task> downloadAction)
        {
            Title = title;
            Content = new SearchContentElementViewModel(new FiltersModel(title, refreshAction), downloadAction);
            DeleteCommand = new MvxAsyncCommand(async () =>
            {
                await deleteAction(this);
            });
        }

        public IMvxAsyncCommand DeleteCommand { get; }
    }
}
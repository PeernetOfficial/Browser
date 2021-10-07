using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Models
{
    public class SearchTabElement : MvxNotifyPropertyChanged
    {
        public SearchTabElement(string title, Action<SearchTabElement> deleteAction, Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction)
        {
            Title = title;
            Content = new SearchContentElement(new FiltersModel(title, refreshAction));
            DeleteCommand = new MvxCommand(() => deleteAction(this));
        }

        public SearchContentElement Content { get; }
        public IMvxCommand DeleteCommand { get; }
        public string Title { get; }
    }
}
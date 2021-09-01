using MvvmCross.Commands;
using MvvmCross.Plugin.FieldBinding;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Services;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        public string _searchInput = string.Empty;
        public string SearchInput 
        { 
            get => _searchInput; 
            set
            { SetProperty(ref _searchInput, value); } 
        }

        public bool _showHint = true;
        public bool ShowHint
        {
            get => _showHint;
            set { SetProperty(ref _showHint, value); }
        }

        public bool _showSearchBox = false;
        public bool ShowSearchBox
        {
            get => _showSearchBox;
            set { SetProperty(ref _showSearchBox, value); }
        }

        public IMvxCommand Search
        {
            get
            {
                return new MvxCommand(() =>
                {
                    SearchInput = "Searching...";
                });
            }
        }

        public IMvxCommand RemoveHint
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (ShowHint)
                    {
                        ShowHint = false;
                        ShowSearchBox = true;
                    }
                });
            }
        } 
    }
}

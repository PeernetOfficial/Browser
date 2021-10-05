using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.ViewModels
{
    public class FiltersViewModel : MvxViewModel<FiltersModel>
    {
        private readonly IMvxNavigationService mvxNavigationService;
        private FiltersModel filters;

        public FiltersViewModel(IMvxNavigationService mvxNavigationService)
        {
            this.mvxNavigationService = mvxNavigationService;
        }

        public FiltersModel Filters
        {
            get => filters;
            set => SetProperty(ref filters, value);
        }

        public override void Prepare(FiltersModel p)
        {
            Filters = p;
            p.CloseAction += Hide;
        }

        private void Hide(bool withApply)
        {
            Filters.CloseAction -= Hide;
            GlobalContext.IsMainWindowActive = true;
            mvxNavigationService.Close(this);
        }
    }
}
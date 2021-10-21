using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Presentation.Home;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class FiltersViewModel : MvxViewModel<FiltersModel>, IModal
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

        private async Task Hide(bool withApply)
        {
            Filters.CloseAction -= Hide;
            GlobalContext.IsMainWindowActive = true;
            await mvxNavigationService.Close(this);
        }
    }
}
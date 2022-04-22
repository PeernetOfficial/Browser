using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Navigation;
using Peernet.SDK.Models.Presentation.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class AdvancedSearchOptionsViewModel : GenericViewModelBase<SearchFilterResultModel>
    {
        private readonly IModalNavigationService modalNavigationService;

        public AdvancedSearchOptionsViewModel(IModalNavigationService modalNavigationService)
        {
            this.modalNavigationService = modalNavigationService;

            SortableColumns = Enum.GetValues<DataGridSortingNameEnum>().Cast<DataGridSortingNameEnum>().ToList();
            SortingDirections = Enum.GetValues<DataGridSortingTypeEnum>().Cast<DataGridSortingTypeEnum>().ToList();
        }

        public IAsyncCommand CloseCommand => new AsyncCommand(() =>
        {
            modalNavigationService.Close();
            return Task.CompletedTask;
        });

        public IAsyncCommand SaveChangesCommand => new AsyncCommand(() =>
        {
            SearchFilter.SortName = SortByColumn;
            SearchFilter.SortType = SortingDirection;
            SearchFilter.IsActive = SortByColumn != DataGridSortingNameEnum.None || SortingDirection != DataGridSortingTypeEnum.None;

            modalNavigationService.Close();
            return Task.CompletedTask;
        });

        public SearchFilterResultModel SearchFilter { get; set; }
        public List<DataGridSortingNameEnum> SortableColumns { get; set; }
        public DataGridSortingNameEnum SortByColumn { get; set; }

        public DataGridSortingTypeEnum SortingDirection { get; set; }
        public List<DataGridSortingTypeEnum> SortingDirections { get; set; }

        public override Task Prepare(SearchFilterResultModel parameter)
        {
            SearchFilter = parameter;

            return Task.CompletedTask;
        }
    }
}
using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Navigation;
using Peernet.SDK.Models.Presentation.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class AdvancedSearchOptionsViewModel : GenericViewModelBase<AdvancedFilterModel>
    {
        private readonly IModalNavigationService modalNavigationService;
        private DataGridSortingNameEnum sortByColumn;
        private DataGridSortingTypeEnum sortingDirection;

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
            AdvancedFilter.SortName = SortByColumn;
            AdvancedFilter.SortType = SortingDirection;
            AdvancedFilter.IsActive = SortByColumn != DataGridSortingNameEnum.None || SortingDirection != DataGridSortingTypeEnum.None;

            modalNavigationService.Close();
            return Task.CompletedTask;
        });

        private AdvancedFilterModel AdvancedFilter;
        public List<DataGridSortingNameEnum> SortableColumns { get; set; }
        public DataGridSortingNameEnum SortByColumn
        {
            get => sortByColumn;
            set
            {
                sortByColumn = value;
                OnPropertyChanged(nameof(SortByColumn));
            }
        }

        public DataGridSortingTypeEnum SortingDirection
        {
            get => sortingDirection;
            set
            {
                sortingDirection = value;
                OnPropertyChanged(nameof(SortingDirection));
            }
        }
        public List<DataGridSortingTypeEnum> SortingDirections { get; set; }

        public override Task Prepare(AdvancedFilterModel parameter)
        {
            AdvancedFilter = parameter;

            return Task.CompletedTask;
        }
    }
}
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Models.Presentation.Home;
using System.ComponentModel;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for SearchResultTabContent.xaml
    /// </summary>
    public partial class SearchResultTabContent : UserControl
    {
        public SearchResultTabContent()
        {
            InitializeComponent();
        }

        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
            var column = e.Column;
            var direction = (column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
            column.SortDirection = direction;
            if (DataContext is SearchContentElementViewModel model)
            {
                model.OnSorting(e.Column.SortMemberPath, direction == ListSortDirection.Ascending ? DataGridSortingTypeEnum.Asc : DataGridSortingTypeEnum.Desc);
            }
        }
    }
}
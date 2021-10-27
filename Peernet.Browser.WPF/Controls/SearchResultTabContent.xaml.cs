using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Models.Presentation.Home;
using System.ComponentModel;
using System.Windows;
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

        private async void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
            var column = e.Column;
            var direction = (column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
            column.SortDirection = direction;
            if (DataContext is SearchTabElementViewModel model)
            {
                await model.OnSorting(e.Column.SortMemberPath, direction == ListSortDirection.Ascending ? DataGridSortingTypeEnum.Asc : DataGridSortingTypeEnum.Desc);
            }
        }

        private void TextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var h = 48;
            var top = 140;
            var t = sender as Grid;
            var position = t.TransformToAncestor(FileGrid).Transform(new Point(0d, 0d));
            var transformation = position.Y - h;
            MapPanel.Visibility = Visibility.Visible;
            var m = new Thickness(MapPanel.Margin.Left, top + transformation, MapPanel.Margin.Right, MapPanel.Margin.Bottom);
            MapPanel.Margin = m;
        }

        private void TextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MapPanel.Visibility = Visibility.Collapsed;
        }
    }
}
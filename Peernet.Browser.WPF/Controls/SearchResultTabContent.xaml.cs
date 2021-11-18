using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Models.Presentation.Home;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Peernet.Browser.WPF.Extensions;

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
            App.MainWindowClicked += OnMainWindowClicked;
        }

        private void OnMainWindowClicked(object sender, RoutedEventArgs e)
        {
            var filterIconControl = ((DependencyObject)e.OriginalSource).FindParent<FilterIconControl>();
            var filterType = (filterIconControl?.DataContext as IconModel)?.FilterType;
            var columnsControl = ((DependencyObject)e.OriginalSource).FindParent<ColumnsSelectorControl>();
            if (columnsControl == null && filterType == null)
            {
                if (DataContext is SearchTabElementViewModel viewModel)
                {
                    viewModel.ShowColumnsSelector = false;
                    viewModel.ColumnsIconModel.IsSelected = false;
                }
            }
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
            var t = sender as FrameworkElement;
            t.Tag = true;
            var position = t.TransformToAncestor(FileGrid).Transform(new Point(0d, 0d));
            var transformation = position.Y - h;
            MapPanel.Visibility = Visibility.Visible;
            var m = new Thickness(MapPanel.Margin.Left, top + transformation, MapPanel.Margin.Right, MapPanel.Margin.Bottom);
            MapPanel.Margin = m;
        }

        private void TextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MapPanel.Visibility = Visibility.Collapsed;
            var t = sender as FrameworkElement;
            t.Tag = false;
        }

        private async void FileGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var viewer = GetScrollViewer((DataGrid)sender);
            if (viewer.VerticalOffset + viewer.ViewportHeight == viewer.ExtentHeight)
            {
                if (DataContext is SearchTabElementViewModel viewModel)
                {
                    await viewModel.IsScrollEnd();
                }
            }
        }

        private ScrollViewer GetScrollViewer(UIElement element)
        {
            if (element == null) return null;

            ScrollViewer retour = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element) && retour == null; i++)
            {
                if (VisualTreeHelper.GetChild(element, i) is ScrollViewer)
                {
                    retour = (ScrollViewer)(VisualTreeHelper.GetChild(element, i));
                }
                else
                {
                    retour = GetScrollViewer(VisualTreeHelper.GetChild(element, i) as UIElement);
                }
            }
            return retour;
        }
    }
}
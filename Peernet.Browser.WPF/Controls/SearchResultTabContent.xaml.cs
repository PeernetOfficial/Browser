using Peernet.Browser.Application.ViewModels;
using Peernet.SDK.Models.Presentation.Home;
using Peernet.Browser.WPF.Extensions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.SDK.Models.Presentation.Footer;

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

        private void OnMainWindowClicked(object sender, RoutedEventArgs e)
        {
            var filterIconControl = ((DependencyObject)e.OriginalSource).FindParent<FilterIconControl>();
            var filterType = (filterIconControl?.DataContext as IconModel)?.FilterType;
            var columnsControl = ((DependencyObject)e.OriginalSource).FindParent<ColumnsSelectorControl>();
            var filtersControl = ((DependencyObject)e.OriginalSource).FindParent<FiltersControl>();
            if (columnsControl == null && filtersControl == null && filterType == null)
            {
                if (DataContext is SearchTabElementViewModel viewModel)
                {
                    viewModel.ColumnsIconModel.IsSelected = false;
                    viewModel.FiltersIconModel.IsSelected = false;
                }
            }
        }

        private void Open_OnClick(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var model = (SearchResultRowModel)element.DataContext;
            var downloadManager = App.ServiceProvider.GetRequiredService<IDownloadManager>();
            var param = new FilePreviewViewModelParameter(model.File, false, async () => await downloadManager.QueueUpDownload(new DownloadModel(model.File)), "Download");
            var filePreviewViewModel = new FilePreviewViewModel();
            filePreviewViewModel.Prepare(param);
            new FilePreviewWindow(filePreviewViewModel).Show();
        }
    }
}
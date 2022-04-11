using DevExpress.Xpf.Grid;
using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.WPF.Extensions;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.Models.Presentation.Home;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            SearchResultsTable.HiddenColumnChooser += SearchResultsTable_ColumnChooserStateChanged;
            SearchResultsTable.ShownColumnChooser += SearchResultsTable_ColumnChooserStateChanged;
            App.MainWindowClicked += OnMainWindowClicked;
        }

        private void SearchResultsTable_ColumnChooserStateChanged(object sender, RoutedEventArgs e)
        {
            ((SearchTabElementViewModel)DataContext).ColumnsIconModel.IsSelected = SearchResultsTable.IsColumnChooserVisible;
        }

        private async void FileGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var viewer = GetScrollViewer((GridControl)sender);
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
                    SearchResultsTable.IsColumnChooserVisible = false;
                    viewModel.FiltersIconModel.IsSelected = false;
                }
            }
        }

        private void Open_OnClick(object sender, MouseButtonEventArgs e)
        {
            var cellData = (EditGridCellData)((FrameworkElement)e.OriginalSource).DataContext;
            var model = (SearchResultRowModel)cellData.RowData.Row;
            var downloadManager = App.ServiceProvider.GetRequiredService<IDownloadManager>();
            var param = new FilePreviewViewModelParameter(model.File, async () => await downloadManager.QueueUpDownload(new DownloadModel(model.File)), "Download");
            var filePreviewViewModel = new FilePreviewViewModel();
            filePreviewViewModel.Prepare(param);
            new FilePreviewWindow(filePreviewViewModel).Show();
        }

        private void FilterIconControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchResultsTable.IsColumnChooserVisible ^= true;
        }
    }
}
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.WPF.Controls;
using Peernet.Browser.WPF.Extensions;
using Peernet.SDK.Models.Presentation.Home;
using System.Windows;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for DictionaryView.xaml
    /// </summary>
    public partial class DirectoryView : UserControl
    {
        public DirectoryView()
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
                if (DataContext is DirectoryViewModel viewModel)
                {
                    viewModel.ColumnsIconModel.IsSelected = false;
                }
            }
        }
    }
}
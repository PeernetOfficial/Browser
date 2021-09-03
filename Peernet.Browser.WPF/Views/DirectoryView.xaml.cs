using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for DictionaryView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(DirectoryViewModel))]
    public partial class DictionaryView : MvxWpfView
    {
        public DictionaryView()
        {
            InitializeComponent();
        }
    }
}
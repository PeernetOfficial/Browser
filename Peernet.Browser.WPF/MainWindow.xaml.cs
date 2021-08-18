using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //[MvxViewFor(typeof(MainViewModel))]
    public partial class MainWindow : MvxWindow
    {
        public MainWindow() => InitializeComponent();
    }
}
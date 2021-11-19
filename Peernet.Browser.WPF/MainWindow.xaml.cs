using MvvmCross.Platforms.Wpf.Views;
using Peernet.Browser.Application;
using Peernet.Browser.Application.Contexts;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using MvvmCross;
using MvvmCross.Navigation;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvxWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MouseDown += Window_MouseDown;

            //Hack for calendar
            CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
            ci.DateTimeFormat.ShortestDayNames = new string[] { "Mo", "Tu", "We", "Th", "Fr", "Sa", "Su" };
            ci.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Sunday;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (newContent is IModal)
            {
                GlobalContext.Modal = newContent;
                Content = oldContent;
                return;
            }
            base.OnContentChanged(oldContent, newContent);
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            App.RaiseMainWindowClick(sender, e);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<HomeViewModel>();
            GlobalContext.CurrentViewModel = nameof(HomeViewModel);
        }
    }
}
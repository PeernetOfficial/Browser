using MvvmCross.Platforms.Wpf.Views;
using Peernet.Browser.Application;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.WPF.Controls;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;

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
            ci.DateTimeFormat.FirstDayOfWeek = System.DayOfWeek.Sunday;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
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

        private void DismissModals_OnClick(object sender, MouseButtonEventArgs e)
        {
            var mouseClickTarget = Mouse.DirectlyOver as FrameworkElement;
            var clickedElement = mouseClickTarget?.Parent as FrameworkElement;
            if (clickedElement is not ProfileMenuControl && clickedElement?.Parent is not ToggleSwitch && (clickedElement?.Parent as FrameworkElement)?.Parent is not ToggleSwitch)
            {
                GlobalContext.IsProfileMenuVisible = false;
            }
        }
    }
}
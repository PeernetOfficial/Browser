using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Presentation.Footer;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for NotificationControl.xaml
    /// </summary>
    public partial class NotificationControl : UserControl
    {
        public static readonly DependencyProperty NotificationProperty =
            DependencyProperty.Register("Notification", typeof(Notification),
                typeof(NotificationControl), null);

        public NotificationControl()
        {
            InitializeComponent();
        }

        public Notification Notification
        {
            get => (Notification)GetValue(NotificationProperty);
            set => SetValue(NotificationProperty, value);
        }

        private void RemoveNotification(object sender, RoutedEventArgs e)
        {
            // ToDo: DataContext should be supplied with NotificationsManager (from ViewModel I suppose)
            //var notification = (Notification)((FrameworkElement)e.OriginalSource).DataContext;
            //GlobalContext.Notifications.Remove(Notification);
        }

        private void Header_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (Content.Visibility == Visibility.Visible)
            {
                Content.Visibility = Visibility.Collapsed;
            }
            else if (Content.Visibility == Visibility.Collapsed)
            {
                Content.Visibility = Visibility.Visible;
            }
        }
    }
}
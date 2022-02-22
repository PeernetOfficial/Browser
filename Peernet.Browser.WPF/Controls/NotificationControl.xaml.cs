using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Managers;
using Peernet.SDK.Models.Presentation.Footer;
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
            App.ServiceProvider.GetRequiredService<INotificationsManager>().Notifications.Remove(Notification);
        }

        private void Header_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (Details.Visibility == Visibility.Visible)
            {
                Details.Visibility = Visibility.Collapsed;
            }
            else if (Details.Visibility == Visibility.Collapsed)
            {
                Details.Visibility = Visibility.Visible;
            }
        }
    }
}
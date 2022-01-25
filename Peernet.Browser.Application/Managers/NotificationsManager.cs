using System.ComponentModel;

namespace Peernet.Browser.Application.Managers
{
    public class NotificationsManager : INotifyPropertyChanged, INotificationsManager
    {
        private NotificationCollection notifications;

        public event PropertyChangedEventHandler PropertyChanged;

        public NotificationsManager(NotificationCollection notifications)
        {
            this.notifications = notifications;
        }

        public NotificationCollection Notifications
        {
            get => notifications;
            // ToDo: Most likely setter is not needed at all
            set
            {
                notifications = value;
                PropertyChanged?.Invoke(this, new(nameof(Notifications)));
            }
        }
    }
}

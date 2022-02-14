using System.ComponentModel;

namespace Peernet.Browser.Application.Managers
{
    public interface INotificationsManager
    {
        NotificationCollection Notifications { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}
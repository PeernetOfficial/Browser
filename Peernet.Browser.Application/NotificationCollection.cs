using Microsoft.Extensions.Logging;
using MvvmCross;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Presentation.Footer;
using System.Collections.ObjectModel;
using System.Threading;

namespace Peernet.Browser.Application
{
    public class NotificationCollection : ObservableCollection<Notification>
    {
        private readonly int timeout;
        private readonly ILogger<NotificationCollection> logger = Mvx.IoCProvider.Resolve<ILogger<NotificationCollection>>();

        public NotificationCollection(int timeout)
        {
            this.timeout = timeout;
        }

        protected override void InsertItem(int index, Notification item)
        {
            var autoEvent = new AutoResetEvent(false);

            item.Timer = new Timer(
                state => { GlobalContext.UiThreadDispatcher?.ExecuteOnMainThreadAsync(() => Remove(item)); },
                autoEvent, timeout, 3000);

            AddLog(item);

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Items[index].Timer.Dispose();
            base.RemoveItem(index);
        }

        private void AddLog(Notification notification)
        {
            var standardLogMessage = notification.Message + $"\n{notification.Details}";
            switch (notification.Severity)
            {
                case Severity.Error:
                    if (notification.Exception != null)
                    {
                        logger.LogError(notification.Exception, notification.Message);
                    }
                    else
                    {
                        logger.LogError(standardLogMessage);
                    }

                    break;
                case Severity.Normal:
                    logger.LogInformation(standardLogMessage);
                    break;
                default:
                    logger.LogDebug(standardLogMessage);
                    break;
            }
        }
    }
}
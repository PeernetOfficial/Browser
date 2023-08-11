using Peernet.Browser.Application.Dispatchers;
using Peernet.SDK.Models.Presentation.Footer;
using Serilog;
using System.Collections.ObjectModel;
using System.Threading;

namespace Peernet.Browser.Application
{
    public class NotificationCollection : ObservableCollection<Notification>
    {
        private readonly int timeout = 11000;
        private readonly ILogger logger;

        public NotificationCollection(ILogger logger)
        {
            this.logger = logger;
        }

        protected override void InsertItem(int index, Notification item)
        {
            var autoEvent = new AutoResetEvent(false);

            item.Timer = new Timer(
                state => { UIThreadDispatcher.ExecuteOnMainThread(() => Remove(item)); },
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
                        logger.Error(notification.Exception, notification.Message);
                    }
                    else
                    {
                        logger.Error(standardLogMessage);
                    }
                    break;

                case Severity.Warning:

                    if (notification.Exception != null)
                    {
                        logger.Warning(notification.Exception, notification.Message);
                    }
                    else
                    {
                        logger.Warning(standardLogMessage);
                    }
                    break;

                case Severity.Normal:
                    logger.Information(standardLogMessage);
                    break;

                default:
                    logger.Debug(standardLogMessage);
                    break;
            }
        }
    }
}
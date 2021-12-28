using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Presentation.Footer;
using System.Collections.ObjectModel;
using System.Threading;

namespace Peernet.Browser.Application
{
    public class NotificationCollection : ObservableCollection<Notification>
    {
        private readonly int timeout;

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

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Items[index].Timer.Dispose();
            base.RemoveItem(index);
        }
    }
}
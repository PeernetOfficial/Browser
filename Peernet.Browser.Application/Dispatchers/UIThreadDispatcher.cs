using System;
using System.Threading;

namespace Peernet.Browser.Application.Dispatchers
{
    public class UIThreadDispatcher : IUIThreadDispatcher
    {
        private readonly SynchronizationContext context;

        public UIThreadDispatcher(SynchronizationContext synchronizationContext)
        {
            context = synchronizationContext;
        }

        public void ExecuteOnMainThread(Action callback)
        {
            context?.Post(new SendOrPostCallback(o => callback?.Invoke()), null);
        }
    }
}

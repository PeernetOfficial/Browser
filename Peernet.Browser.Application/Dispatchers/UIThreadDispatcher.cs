using System;
using System.Threading;

namespace Peernet.Browser.Application.Dispatchers
{
    public static class UIThreadDispatcher
    {
        private static SynchronizationContext Context;

        public static void SetUIContext(SynchronizationContext context)
        {
            Context = context;
        }

        public static void ExecuteOnMainThread(Action callback)
        {
            Context?.Send(new SendOrPostCallback(o => callback?.Invoke()), null);
        }
    }
}
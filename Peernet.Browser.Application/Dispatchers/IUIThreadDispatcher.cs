using System;

namespace Peernet.Browser.Application.Dispatchers
{
    public interface IUIThreadDispatcher
    {
        void ExecuteOnMainThread(Action callback);
    }
}

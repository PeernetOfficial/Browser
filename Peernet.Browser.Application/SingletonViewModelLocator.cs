using MvvmCross;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Clients;
using Peernet.Browser.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Peernet.Browser.Application
{
    public class SingletonViewModelLocator : MvxDefaultViewModelLocator
    {
        private IDictionary<Type, IMvxViewModel> Container = new Dictionary<Type, IMvxViewModel>();

        public override IMvxViewModel Load(Type viewModelType, IMvxBundle parameterValues, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs = null)
        {
            if (viewModelType == typeof(TerminalViewModel))
            {
                return new TerminalViewModel(Mvx.IoCProvider.Resolve<ISocketClient>());
            }

            if (!Container.ContainsKey(viewModelType))
            {
                var res = base.Load(viewModelType, parameterValues, savedState, navigationArgs);
                Container.Add(viewModelType, res);
            }
            return Container[viewModelType];
        }
    }
}
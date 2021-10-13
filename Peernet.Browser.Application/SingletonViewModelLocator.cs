using MvvmCross.Navigation.EventArguments;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;

namespace Peernet.Browser.Application
{
    public class SingletonViewModelLocator : MvxDefaultViewModelLocator
    {
        private IDictionary<Type, IMvxViewModel> Container = new Dictionary<Type, IMvxViewModel>();

        public override IMvxViewModel Load(Type viewModelType, IMvxBundle parameterValues, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs = null)
        {
            if (!Container.ContainsKey(viewModelType))
            {
                var res = base.Load(viewModelType, parameterValues, savedState, navigationArgs);
                Container.Add(viewModelType, res);
            }
            return Container[viewModelType];
        }
    }
}
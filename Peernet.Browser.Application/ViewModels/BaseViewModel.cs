using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class BaseViewModel : MvxNavigationViewModel
    {
        public BaseViewModel(ILoggerFactory loggerFactory, IMvxNavigationService navigationService)
            : base(loggerFactory, navigationService)
        {

        }
    }
}

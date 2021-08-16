using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peernet.Browser.Application
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<StatusViewModel>();
            RegisterAppStart<HomeViewModel>();
        }
    }
}

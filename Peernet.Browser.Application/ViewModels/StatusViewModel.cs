using MvvmCross.ViewModels;
using Peernet.Browser.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class StatusViewModel : MvxViewModel
    {
        private readonly IApiClient apiClient;

        public StatusViewModel(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }


    }
}

using System;
using System.Collections.Generic;
using MvvmCross.ViewModels;
using Peernet.Browser.Models.Domain.Common;

namespace Peernet.Browser.Application.ViewModels
{
    public class AboutViewModel : MvxViewModel
    {
        public List<ApiFile> Files { get; set; }

        public AboutViewModel()
        {
            Files = new List<ApiFile>
            {
                new()
                {
                    Name = "FileA",
                    Size = 321030,
                    Date = DateTime.Now,
                },
                new()
                {
                    Name = "FileB",
                    Size = 321032,
                    Date = DateTime.Now,
                },
                new()
                {
                    Name = "FileC",
                    Size = 321033,
                    Date = DateTime.Now,
                }

            };
        }
    }
}
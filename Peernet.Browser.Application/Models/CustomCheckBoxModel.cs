﻿using MvvmCross.ViewModels;
using System;

namespace Peernet.Browser.Application.Models
{
    public class CustomCheckBoxModel : MvxNotifyPropertyChanged
    {
        private bool isChecked;

        private string content;

        public Action<CustomCheckBoxModel> IsCheckChanged { get; set; }

        public bool IsChecked

        {
            get => isChecked;
            set
            {
                if (SetProperty(ref isChecked, value)) IsCheckChanged?.Invoke(this);
            }
        }

        public string Content

        {
            get => content;
            set => SetProperty(ref content, value);
        }
    }
}
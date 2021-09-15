using MvvmCross.ViewModels;
using System;

namespace Peernet.Browser.Application.Models
{
    public class CustomCheckBoxModel : MvxNotifyPropertyChanged
    {
        private bool isChecked;

        private string value;

        public Action<CustomCheckBoxModel> IsCheckChanged { get; set; }

        public bool IsChecked

        {
            get => isChecked;
            set
            {
                SetProperty(ref isChecked, value);
                IsCheckChanged?.Invoke(this);
            }
        }

        public bool ResetAll { get; set; }

        public string Value

        {
            get => value;
            set => SetProperty(ref value, value);
        }
    }
}
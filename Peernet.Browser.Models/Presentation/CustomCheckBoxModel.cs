using MvvmCross.ViewModels;
using System;

namespace Peernet.Browser.Models.Presentation
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

        public Enum EnumerationMember { get; set; }

        public string Content

        {
            get => content;
            set => SetProperty(ref content, value);
        }

        public bool ShowDot { get; set; }
    }
}
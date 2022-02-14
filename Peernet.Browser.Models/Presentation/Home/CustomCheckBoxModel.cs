using System;
using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class CustomCheckBoxModel : INotifyPropertyChanged
    {
        private bool isChecked;

        private string content;

        public Action<CustomCheckBoxModel> IsCheckChanged { get; set; }

        public bool IsChecked

        {
            get => isChecked;
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    PropertyChanged?.Invoke(this, new(nameof(IsChecked)));
                    IsCheckChanged?.Invoke(this);
                }
            }
        }

        public Enum EnumerationMember { get; set; }

        public string Content

        {
            get => content;
            set
            {
                content = value;
                PropertyChanged?.Invoke(this, new(nameof(Content)));
            }
        }

        public bool IsRadio { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
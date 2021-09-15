using MvvmCross.ViewModels;
using System;
using System.Linq;

namespace Peernet.Browser.Application.Models
{
    public class CustomFilterModel : MvxNotifyPropertyChanged
    {
        public CustomFilterModel(string[] elements, bool firstReset = true)
        {
            if (elements.IsNullOrEmpty()) throw new ArgumentException("elements");
            Items.AddRange(elements.Select(x => new CustomCheckBoxModel { Value = x }));
            if (firstReset)
            {
                var first = Items.First();
                first.ResetAll = true;
                first.IsCheckChanged = IsChekcedChanged;
            }
        }

        public MvxObservableCollection<CustomCheckBoxModel> Items { get; } = new MvxObservableCollection<CustomCheckBoxModel>();

        private void IsChekcedChanged(CustomCheckBoxModel c)
        {
            foreach (var i in Items.Where(x => x != c)) { i.IsChecked = false; }
        }
    }

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
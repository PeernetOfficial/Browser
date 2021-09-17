using MvvmCross.ViewModels;
using System;
using System.Linq;

namespace Peernet.Browser.Application.Models
{

    public class CustomFilterModel : MvxNotifyPropertyChanged
    {
        private readonly CustomCheckBoxModel first;

        public CustomFilterModel(string title, string[] elements, bool firstReset = true)
        {
            Title = title.ToUpper();
            if (elements.IsNullOrEmpty()) throw new ArgumentException("elements");
            Items.AddRange(elements.Select(x => new CustomCheckBoxModel { Content = x, IsCheckChanged = IsChekcedChanged }));
            if (firstReset) first = Items.First();
        }

        public string Title { get; }

        public MvxObservableCollection<CustomCheckBoxModel> Items { get; } = new MvxObservableCollection<CustomCheckBoxModel>();

        private void IsChekcedChanged(CustomCheckBoxModel c)
        {
            if (first?.IsChecked != true || !c.IsChecked) return;
            var isFirst = first == c;
            if (isFirst) Items.Where(x => x != c).Foreach(x => x.IsChecked = false);
            else first.IsChecked = false;
        }
    }
}
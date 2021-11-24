using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Models.Presentation.Home
{
    public abstract class CustomFilterModel<T> : MvxNotifyPropertyChanged where T : Enum
    {
        private readonly bool isRadio;

        private int selectedItemIndex = 0;

        protected CustomFilterModel(string title, bool isRadio = true)
        {
            this.isRadio = isRadio;
            Title = title;
            Items.AddRange(GetElements()
                .Select(x => new CustomCheckBoxModel
                {
                    EnumerationMember = x.Key,
                    Content = x.Value,
                    IsCheckChanged = IsCheckedChanged,
                    IsRadio = isRadio
                }));
        }

        public bool IsSelected => Items.Any(x => x.IsChecked);

        public MvxObservableCollection<CustomCheckBoxModel> Items { get; } = new();

        public int SelectedItemIndex
        {
            get => selectedItemIndex;
            set
            {
                selectedItemIndex = value;
                SetProperty(ref selectedItemIndex, value);
                RaisePropertyChanged(nameof(SelectedItemIndex));
            }
        }

        public string Title { get; }

        public T GetSelected()
        {
            return (T)Items.ElementAt(SelectedItemIndex).EnumerationMember;
        }

        public void Set(Enum val)
        {
            if (val == null) return;
            foreach (var i in Items)
            {
                if (val.Equals(i.EnumerationMember))
                {
                    i.IsChecked = true;
                }
            }
        }

        public virtual void UnselectAll() => Items.Foreach(x => x.IsChecked = false);

        protected virtual IEnumerable<KeyValuePair<Enum, string>> GetElements()
        {
            var type = typeof(T);
            foreach (T val in Enum.GetValues(type))
            {
                var d = val.GetDescription();
                if (d != null)
                {
                    yield return new KeyValuePair<Enum, string>(val, d);
                }
            }
        }

        protected virtual void IsCheckedChanged(CustomCheckBoxModel c)
        {
            if (c.IsChecked && isRadio)
            {
                Items.Where(x => x != c).Foreach(x => x.IsChecked = false);
            }
        }
    }
}
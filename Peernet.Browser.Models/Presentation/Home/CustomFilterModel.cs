using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Peernet.Browser.Models.Presentation.Home
{
    public abstract class CustomFilterModel<T> : INotifyPropertyChanged where T : Enum
    {
        private readonly bool isRadio;
        private readonly Action onSelectionChanged;
        private int selectedItemIndex = 0;

        protected CustomFilterModel(string title, Action onSelectionChanged, bool isRadio = true)
        {
            this.isRadio = isRadio;
            this.onSelectionChanged = onSelectionChanged;
            Title = title;
            foreach(var element in GetElements().OrderBy(e => e.Key))
            {
                var model = new CustomCheckBoxModel
                {
                    EnumerationMember = element.Key,
                    Content = element.Value,
                    IsCheckChanged = IsCheckedChanged,
                    IsRadio = isRadio
                };

                Items.Add(model);
            }
        }

        public bool IsSelected => Items.Any(x => x.IsChecked);

        public ObservableCollection<CustomCheckBoxModel> Items { get; } = new();

        public int SelectedItemIndex
        {
            get => selectedItemIndex;
            set
            {
                selectedItemIndex = value;
                PropertyChanged?.Invoke(this, new(nameof(SelectedItemIndex)));
                onSelectionChanged?.Invoke();
            }
        }

        public string Title { get; }

        public event PropertyChangedEventHandler PropertyChanged;

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
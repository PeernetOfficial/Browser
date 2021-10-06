using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.Models
{
    public abstract class CustomFilterModel<T> : MvxNotifyPropertyChanged where T : Enum
    {
        protected CustomFilterModel(string title, bool showDot = false)
        {
            Title = title.ToUpper();
            Items.AddRange(GetElements()
                .Select(x => new CustomCheckBoxModel
                {
                    EnumerationMember = x.Key,
                    Content = x.Value,
                    IsCheckChanged = IsCheckedChanged,
                    ShowDot = showDot
                }));
        }

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

        public double MinHeight { get; set; }

        public string Title { get; }

        public MvxObservableCollection<CustomCheckBoxModel> Items { get; } = new MvxObservableCollection<CustomCheckBoxModel>();

        private void IsCheckedChanged(CustomCheckBoxModel c)
        {
            //TODO: will be used int RadioButton type
            //if (c.IsChecked) Items.Where(x => x != c).Foreach(x => x.IsChecked = false);
        }

        public T GetSelected()
        {
            return (T)Items.First(x => x.IsChecked).EnumerationMember;
        }

        public T[] GetAllSelected()
        {
            return Items
                .Where(x => x.IsChecked)
                .Select(x => (T)x.EnumerationMember)
                .ToArray();
        }

        public bool IsSelected => Items.Any(x => x.IsChecked);

        public void Set(T[] vals)
        {
            if (vals.IsNullOrEmpty()) return;
            foreach (var i in Items)
            {
                if (vals.Contains((T)i.EnumerationMember)) i.IsChecked = true;
            }
        }

        public void Set(Enum val)
        {
            if (val == null) return;
            foreach (var i in Items)
            {
                if (val.Equals(i.EnumerationMember)) i.IsChecked = true;
            }
        }

        public void DeselctAll() => Items.Foreach(x => x.IsChecked = false);
    }
}
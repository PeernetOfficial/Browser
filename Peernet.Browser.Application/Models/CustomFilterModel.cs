using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.Models
{
    public abstract class CustomFilterModel<T> : MvxNotifyPropertyChanged where T : Enum
    {
        protected readonly CustomCheckBoxModel first;

        protected CustomFilterModel(string title, bool firstReset = true, bool showDot = false)
        {
            Title = title.ToUpper();
            Items.AddRange(GetElements().Select(x => new CustomCheckBoxModel { EnumerationMember = x.Key, Content = x.Value, IsCheckChanged = IsCheckedChanged, ShowDot = showDot }));
            if (firstReset) first = Items.First();
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

        public string Title { get; }

        public MvxObservableCollection<CustomCheckBoxModel> Items { get; } = new MvxObservableCollection<CustomCheckBoxModel>();

        private void IsCheckedChanged(CustomCheckBoxModel c)
        {
            if (first?.IsChecked != true || !c.IsChecked) return;
            var isFirst = first == c;
            if (isFirst || first == null) Items.Where(x => x != c).Foreach(x => x.IsChecked = false);
            else first.IsChecked = false;
        }

        public T GetSelected()
        {
            return Items.Where(x => x.IsChecked).Select(x => (T)x.EnumerationMember).FirstOrDefault();
        }

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
    }
}
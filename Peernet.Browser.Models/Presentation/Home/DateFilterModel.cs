using System;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class DateFilterModel : CustomFilterModel<TimePeriods>
    {
        private readonly Action<bool> onIsCustomSlectionChange;

        public DateFilterModel(Action<bool> onIsCustomSlectionChange) : base("Date", isRadio: true)
        {
            this.onIsCustomSlectionChange = onIsCustomSlectionChange;
            MinHeight = 241;
        }

        protected override void IsCheckedChanged(CustomCheckBoxModel c)
        {
            base.IsCheckedChanged(c);
            onIsCustomSlectionChange?.Invoke((TimePeriods)c.EnumerationMember == TimePeriods.Custom && c.IsChecked);
        }
    }
}
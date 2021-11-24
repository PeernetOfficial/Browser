using System;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class DateFilterModel : CustomFilterModel<TimePeriods>
    {
        private readonly Action<bool> onIsCustomSelectionChange;

        public DateFilterModel(Action<bool> onIsCustomSelectionChange) : base("Date")
        {
            this.onIsCustomSelectionChange = onIsCustomSelectionChange;
        }

        protected override void IsCheckedChanged(CustomCheckBoxModel customCheckBoxModel)
        {
            base.IsCheckedChanged(customCheckBoxModel);
            // onIsCustomSelectionChange?.Invoke((TimePeriods)customCheckBoxModel.EnumerationMember == TimePeriods.Custom && customCheckBoxModel.IsChecked);
        }

        public override void UnselectAll()
        {
            base.UnselectAll();
            Set(TimePeriods.None);
            SelectedItemIndex = 0;
        }
    }
}
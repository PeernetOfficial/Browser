using System;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class DateFilterModel : CustomFilterModel<TimePeriods>
    {
        public DateFilterModel(Action onSelectionChanged)
            : base("Date", onSelectionChanged)
        {
        }

        public override void UnselectAll()
        {
            base.UnselectAll();
            Set(TimePeriods.None);
            SelectedItemIndex = 0;
        }
    }
}

namespace Peernet.Browser.Models.Presentation.Home
{
    public class DateFilterModel : CustomFilterModel<TimePeriods>
    {
        public DateFilterModel() : base("Date")
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
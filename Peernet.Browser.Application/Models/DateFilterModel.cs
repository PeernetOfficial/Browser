using Peernet.Browser.Application.Enums;

namespace Peernet.Browser.Application.Models
{
    public class DateFilterModel : CustomFilterModel<TimePeriods>
    {
        public DateFilterModel() : base("Date")
        {
            MinHeight = 241;
        }
    }
}
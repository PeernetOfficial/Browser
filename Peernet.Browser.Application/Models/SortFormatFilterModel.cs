using Peernet.Browser.Application.Enums;

namespace Peernet.Browser.Application.Models
{
    public class SortFormatFilterModel : CustomFilterModel<SortOrders>
    {
        public SortFormatFilterModel() : base("Sort order", false)
        {
        }
    }
}
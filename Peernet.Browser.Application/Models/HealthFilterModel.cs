using Peernet.Browser.Application.Enums;

namespace Peernet.Browser.Application.Models
{
    public class HealthFilterModel : CustomFilterModel<HealthType>
    {
        public HealthFilterModel() : base("Sort order", showDot: true)
        {
        }
    }
}
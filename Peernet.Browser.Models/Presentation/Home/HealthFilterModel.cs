namespace Peernet.Browser.Models.Presentation.Home
{
    public class HealthFilterModel : CustomFilterModel<HealthType>
    {
        public HealthFilterModel() : base("Sort order", showDot: true)
        {
        }
    }
}
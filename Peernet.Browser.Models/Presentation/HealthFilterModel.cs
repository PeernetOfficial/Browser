namespace Peernet.Browser.Models.Presentation
{
    public class HealthFilterModel : CustomFilterModel<HealthType>
    {
        public HealthFilterModel() : base("Sort order", showDot: true)
        {
        }
    }
}
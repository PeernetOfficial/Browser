namespace Peernet.Browser.Infrastructure.Wrappers
{
    public abstract class WrapperBase
    {
        public abstract string CoreSegment { get; }

        protected string GetRelativeRequestPath(string consecutiveSegments)
        {
            return string.IsNullOrEmpty(consecutiveSegments) ? CoreSegment : $"{CoreSegment}/{consecutiveSegments}";
        }
    }
}
namespace Peernet.Browser.Infrastructure.Wrappers
{
    internal abstract class WrapperBase
    {
        public abstract string CoreSegment { get; }

        protected string GetRelativeRequestPath(string consecutiveSegments)
        {
            return string.IsNullOrEmpty(consecutiveSegments) ? CoreSegment : $"{CoreSegment}/{consecutiveSegments}";
        }
    }
}
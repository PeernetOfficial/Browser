namespace Peernet.Browser.Infrastructure.Clients
{
    internal abstract class ClientBase
    {
        public abstract string CoreSegment { get; }

        protected string GetRelativeRequestPath(string consecutiveSegments)
        {
            return string.IsNullOrEmpty(consecutiveSegments) ? CoreSegment : $"{CoreSegment}/{consecutiveSegments}";
        }
    }
}
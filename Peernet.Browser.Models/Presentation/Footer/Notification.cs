using System.Threading;

namespace Peernet.Browser.Models.Presentation.Footer
{
    public class Notification
    {
        public Notification(string message, string details = null, Severity severity = Severity.Normal)
        {
            Message = message;
            Details = details ?? string.Empty;
            Severity = severity;
        }

        public string Message { get; init; }

        public string Details { get; set; }

        public Severity Severity { get; init; }

        public Timer Timer { get; set; }
    }
}
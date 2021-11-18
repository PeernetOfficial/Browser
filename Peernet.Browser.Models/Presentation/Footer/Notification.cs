using System.Threading;

namespace Peernet.Browser.Models.Presentation.Footer
{
    public class Notification
    {
        public Notification(string text, Severity severity = Severity.Normal)
        {
            Text = text;
            Severity = severity;
        }

        public string Text { get; init; }

        public Severity Severity { get; init; }

        public Timer Timer { get; set; }
    }
}
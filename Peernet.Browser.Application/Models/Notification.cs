using System;
using System.Threading;

namespace Peernet.Browser.Application.Models
{
    public class Notification
    {
        public string Text { get; set; }

        public Timer Timer { get; set; }
    }
}

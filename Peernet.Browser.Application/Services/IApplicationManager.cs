using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IApplicationManager
    {
        public void Shutdown();
        public void Maximize();
        public void Restore();
        public void Minimize();

        public bool IsMaximized { get; }
    }
}

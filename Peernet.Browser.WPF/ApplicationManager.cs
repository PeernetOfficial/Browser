using Peernet.Browser.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peernet.Browser.WPF
{
    public class ApplicationManager : IApplicationManager
    {
        private MainWindow window;

        
        public bool IsMaximized 
        {
            get
            {
                if (this.window == null)
                {
                    this.window = (MainWindow)(System.Windows.Application.Current.MainWindow);
                }

                return this.window.WindowState == System.Windows.WindowState.Maximized;
            }
        }

        public void Maximize()
        {
            if (this.window == null)
            {
                this.window = (MainWindow)(System.Windows.Application.Current.MainWindow);
            }

            this.window.WindowState = System.Windows.WindowState.Maximized;
        }

        public void Minimize()
        {
            if (this.window == null)
            {
                this.window = (MainWindow)(System.Windows.Application.Current.MainWindow);
            }

            this.window.WindowState = System.Windows.WindowState.Minimized;
        }

        public void Shutdown()
        {
            if (this.window == null)
            {
                this.window = (MainWindow)(System.Windows.Application.Current.MainWindow);
            }

            this.window.Close();
        }

        public void Restore()
        {
            if (this.window == null)
            {
                this.window = (MainWindow)(System.Windows.Application.Current.MainWindow);
            }

            this.window.WindowState = System.Windows.WindowState.Normal;
        }
    }
}
